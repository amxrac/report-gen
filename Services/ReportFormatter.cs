using rgproj.Services;
using System.Text.RegularExpressions;

public class ReportFormatter : IReportFormatter
{
    public string FormatReport(string content)
    {
        if (string.IsNullOrEmpty(content))
            return string.Empty;

        var styles = @"
            <style>
                .report-container{max-width:1000px;margin:2rem auto;padding:2rem;font-family:system-ui,-apple-system,sans-serif;line-height:1.5}
                .report-header{margin-bottom:2rem;border-bottom:1px solid #e5e7eb;padding-bottom:1rem}
                .section{margin:2rem 0}
                .section-title{font-size:1.5rem;font-weight:600;margin:1.5rem 0 1rem}
                .subsection-title{font-size:1.25rem;font-weight:500;margin:1rem 0;color:#374151}
                .numbered-list{list-style-type:decimal;margin:1rem 0;padding-left:2rem}
                .numbered-list li{margin:0.75rem 0}
                .nested-list{list-style-type:disc;margin:0.5rem 0 0.5rem 2rem}
                .nested-item{margin:0.5rem 0}
                .metadata{color:#666;font-size:0.9em}
                .priority-item{font-weight:500}
            </style>";

        // Clean up special characters and formatting
        content = content
            .Replace("\\n", "\n")
            .Replace("\\t", "    ")
            .Replace("\n\n", "<div class='section'>")
            .Replace("* ", "• ");

        // Remove metadata tags
        content = Regex.Replace(content, @"\(VET: [^\)]+\)", "<span class='metadata'>$0</span>");
        content = Regex.Replace(content, @"\(SPEC: [^\)]+\)", "<span class='metadata'>$0</span>");

        // Format headers
        content = Regex.Replace(content, @"^One Health Report", "<h1 class='report-title'>$0</h1>", RegexOptions.Multiline);

        // Format section titles
        content = Regex.Replace(
            content,
            @"(Executive Summary|Key Findings|Critical Intersections|Risk Assessment|Action Plan|Recommendations)(?:\s*\([^)]*\))?:",
            "<h2 class='section-title'>$1</h2>");

        // Format subsection titles
        content = Regex.Replace(
            content,
            @"(\d+\.\s*)?(Environmental|Veterinary|Public Health) Risks:",
            "<h3 class='subsection-title'>$2 Risks</h3>");

        // Convert numbered lists
        content = Regex.Replace(
            content,
            @"(?m)^\d+\.\s+(.+)$",
            "<li>$1</li>");

        // Wrap consecutive numbered items
        content = Regex.Replace(
            content,
            @"(<li>.+?</li>)\s*(<li>.+?</li>)+",
            "<ol class='numbered-list'>$0</ol>");

        // Handle nested bullet points
        content = Regex.Replace(
            content,
            @"\+ ([^\n]+)",
            "<li class='nested-item'>$1</li>");

        // Wrap nested items in ul
        content = Regex.Replace(
            content,
            @"(<li class='nested-item'>.+?</li>)\s*(<li class='nested-item'>.+?</li>)+",
            "<ul class='nested-list'>$0</ul>");

        // Clean up any remaining newlines
        content = content.Replace("\n", "<br>");

        return $"{styles}<div class='report-container'>{content}</div>";
    }
}