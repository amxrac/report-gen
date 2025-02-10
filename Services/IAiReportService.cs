using System.Buffers.Text;
using System.Net.Http;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using rgproj.Data;
using rgproj.Models;

namespace rgproj.Services
{
    public interface IAiReportService
    {
        Task<string> GenerateComprehensiveReportAsync(List<object> forms, string modelType, bool isPublic);
        Task<string> GenerateWithOllama(string prompt);
    }

    public class ReportService : IAiReportService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public ReportService(HttpClient httpClient, IConfiguration config, AppDbContext context)
        {
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15),
                KeepAlivePingPolicy = HttpKeepAlivePingPolicy.WithActiveRequests,
                EnableMultipleHttp2Connections = true,
                ConnectTimeout = TimeSpan.FromSeconds(30),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 10
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMinutes(30),
                
            };

            _config = config;
            _context = context;
        }

        public async Task<string> GenerateComprehensiveReportAsync(List<object> forms, string modelType, bool isPublic)
        {
            if (forms == null) throw new ArgumentNullException(nameof(forms));
            if (string.IsNullOrWhiteSpace(modelType))
                throw new ArgumentException("Model type must be specified");
            if (forms == null || !forms.Any())
                throw new ArgumentException("No forms selected for report generation");

            if (forms.Count > 10)
                throw new ArgumentException("Maximum of 10 forms allowed per report");

            var reportData = new StringBuilder();
            int formCount = 0;
            foreach (var form in forms)
            {
                if (formCount++ >= 5) break;
                switch (form)
                {
                    case EnvironmentalistForm envForm:
                        reportData.AppendLine(FormatEnvironmentalistData(envForm));
                        break;
                    case VeterinaryDoctorForm vetForm:
                        reportData.AppendLine(FormatVeterinaryData(vetForm));
                        break;
                    case SpecialistForm specForm:
                        reportData.AppendLine(FormatSpecialistData(specForm));
                        break;
                    case HealthOfficerForm healthForm:
                        reportData.AppendLine(FormatHealthOfficerData(healthForm));
                        break;

                }
            }

            if (formCount > 5)
            {
                reportData.AppendLine($"\nNote: Summarized {formCount - 5} additional reports for brevity.");
            }

            var prompt = string.Format(ReportTemplates.MultiForm,
                DateTime.Now.ToString("yyyy-MM-dd"),
                reportData.ToString(),
                isPublic ? "public health advisory" : "internal use only");

            Console.WriteLine("Generated Prompt:");
            Console.WriteLine(prompt);

            const int MaxPromptLength = 1500; 
            if (prompt.Length > MaxPromptLength)
            {
                var truncated = new StringBuilder(prompt.Substring(0, MaxPromptLength - 100));
                truncated.AppendLine("\n[Note: Some details truncated for length]");
                prompt = truncated.ToString();
            }

            return modelType.ToLower() switch
            {
                "gemini" => await GenerateWithGemini(prompt),
                "ollama" => await GenerateWithOllama(prompt),
                _ => throw new ArgumentException("Invalid model type")
            };
        }

        private string ShortenText(string text, int wordLimit = 20)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            var words = text.Split(' ').Take(wordLimit);
            var shortened = string.Join(" ", words);
            return shortened.Length < text.Length ? shortened + "..." : shortened;
        }

        private string FormatEnvironmentalistData(EnvironmentalistForm form)
        {
            return $"ENV: {form.Location} - {form.Category}\n" +
               $"Key: {ShortenText(form.Description, 15)}\n" +
               $"Action: {ShortenText(form.ActionsTaken, 10)}";
        }

        private string FormatVeterinaryData(VeterinaryDoctorForm form)
        {
            return $"VET: {form.AnimalSpecies} ({form.HealthStatus})\n" +
               $"Signs: {ShortenText(form.ClinicalSymptoms, 10)}\n" +
               $"Zoonotic: {(form.PotentialZoonoticRisk ? "Yes" : "No")}";
        }

        private string FormatSpecialistData(SpecialistForm form)
        {
            return $"SPEC: {form.CaseType} (Severity: {form.SeverityLevel})\n" +
               $"Transmission: {ShortenText(form.TransmissionPattern, 10)}";
        }

        private string FormatHealthOfficerData(HealthOfficerForm form)
        {
            return $"HEALTH: {form.FacilityType} ({form.SanitationStatus})\n" +
               $"Status: {ShortenText(form.InspectionResults, 10)}";
        }

        private async Task<string> GenerateWithGemini(string prompt)
        {
            var apiKey = _config["GeminiApi:ApiKey"];
            var url = $"https://generativelanguage.googleapis.com/v1/models/gemini-pro:generateContent?key={apiKey}";

            var request = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 2048
                }
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, request);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadFromJsonAsync<JsonDocument>();
                return jsonResponse?.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString() ?? "No content generated";
            }
            catch (Exception ex)
            {
                return $"Error generating report: {ex.Message}";
            }
        }

        private class OllamaResponse
        {
            public string? Response { get; set; }
        }

        public async Task<string> GenerateWithOllama(string prompt)
        {
            try
            {
                var flaskUrl = "http://localhost:5000/generate";

                var jsonContent = JsonSerializer.Serialize(new { prompt });
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                Console.WriteLine("Sending request to Flask service...");
                using var response = await _httpClient.PostAsync(flaskUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Raw Response: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Error: {response.StatusCode} - {responseContent}");
                    return $"API Error: {response.StatusCode} - {responseContent}";
                }

                var startIndex = responseContent.IndexOf("One Health Report", StringComparison.OrdinalIgnoreCase);
                if (startIndex == -1)
                    return "Error: Could not locate the start of the report.";

                var cleanResponse = responseContent[startIndex..];

                var footerIndex = cleanResponse.IndexOf("Prepared by:", StringComparison.OrdinalIgnoreCase);
                if (footerIndex != -1)
                    cleanResponse = cleanResponse[..footerIndex].Trim();

                Console.WriteLine($"Cleaned Response: {cleanResponse}");

                return cleanResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return $"Generation error: {ex.Message}";
            }
        }


        public static class ReportTemplates
        {
            public const string MultiForm = @"
                Generate a comprehensive One Health report for the following data. Be concise and focused.
                - Do not include thoughts or reasoning steps in the response.
                - Directly structure the report based on the required sections.
                - Keep responses professional and concise.

                Context:
                - Date: {0}
                - Audience: {2}
                - Purpose: One Health assessment and recommendations

                Input Data:
                {1}

                Required Sections:
                1. Executive Summary (Max 300 words)
                   - Key findings
                   - Critical intersections

                2. Risk Assessment
                   - Environmental: [priority risks]
                   - Animal Health: [key concerns]
                   - Human Health: [major factors]

                3. Recommendations
                   - Critical actions
                   - Prevention strategies
                   - Community measures

                Guidelines:
                - Focus on critical insights
                - Use clear, professional language
                - Prioritize actionable items
            ";
        }
    }
}