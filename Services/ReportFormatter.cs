using rgproj.Services;
using System.Text.RegularExpressions;

public class ReportFormatter : IReportFormatter
{
    public string FormatReport(string content)
    {
        if (string.IsNullOrEmpty(content))
            return string.Empty;

        var formattedContent = content
            // Format headers
            .Replace("### ", "<h3>")
            .Replace("#### ", "<h4>")

            // Format sections
            .Replace("---", "<hr>")

            // Format lists
            .Replace("\n- ", "<br>• ")
            .Replace("\n1. ", "<br>1. ")
            .Replace("\n2. ", "<br>2. ")
            .Replace("\n3. ", "<br>3. ")

            // Format line breaks
            .Replace("\n", "<br>");

        // Add closing tags for headers
        formattedContent = Regex.Replace(formattedContent, "<h3>(.*?)<br>", "<h3>$1</h3>");
        formattedContent = Regex.Replace(formattedContent, "<h4>(.*?)<br>", "<h4>$1</h4>");

        return $@"
            <div class='formatted-report'>
                {formattedContent}
            </div>";
    }
}