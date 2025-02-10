using System.Text.RegularExpressions;
using System.Web;

namespace rgproj.Services
{
    public class ReportFormatter : IReportFormatter
    {
        public string FormatReport(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            // Escape HTML to prevent injection issues
            content = HttpUtility.HtmlEncode(content);

            // Convert basic markdown-like structure to HTML
            content = Regex.Replace(content, @"^### (.+)$", "<h3>$1</h3>", RegexOptions.Multiline);
            content = Regex.Replace(content, @"^#### (.+)$", "<h4>$1</h4>", RegexOptions.Multiline);
            content = Regex.Replace(content, @"^- (.+)$", "<li>$1</li>", RegexOptions.Multiline);
            content = Regex.Replace(content, @"^\d+\. (.+)$", "<li>$1</li>", RegexOptions.Multiline);

            // Wrap lists in <ul> or <ol>
            content = Regex.Replace(content, @"(<li>.*?</li>)", "<ul>$1</ul>");
            content = Regex.Replace(content, @"(<ul><li>\d+\..*?</li></ul>)", "<ol>$1</ol>");

            // Paragraph formatting
            content = Regex.Replace(content, @"\n{2,}", "</p><p>");
            content = $"<p>{content}</p>";

            // Final formatting
            return $"<div class='formatted-report'>{content}</div>";
        }
    }
}
