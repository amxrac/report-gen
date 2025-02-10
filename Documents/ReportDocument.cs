using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using rgproj.Models;
namespace rgproj.Documents
{
    public class ReportDocument : IDocument
    {
        private readonly GeneratedReport _report;
        public ReportDocument(GeneratedReport report)
        {
            _report = report ?? throw new ArgumentNullException(nameof(report));
        }
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(12));
                page.Header()
                    .Column(column =>
                    {
                        column.Item().Text("One Health Comprehensive Report")
                            .Bold().FontSize(20).AlignCenter();
                        column.Item().PaddingTop(5)
                            .Text($"Generated: {_report.GeneratedDate:MMMM dd, yyyy}")
                            .FontSize(10).AlignCenter();
                        column.Item().PaddingTop(5)
                            .Text($"Model Used: {_report.ModelUsed}")
                            .FontSize(10).AlignCenter();
                    });
                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        if (!_report.IsPublic)
                        {
                            column.Item().Background(Colors.Grey.Lighten3)
                                .Padding(5)
                                .Text("INTERNAL USE ONLY")
                                .Bold()
                                .AlignCenter();
                        }
                        column.Item().Text(_report.Content
                            .Replace("**", "")
                            .Replace("*", "")
                            .Replace("#", "")
                        );
                    });
                page.Footer()
                    .AlignCenter()
                    .Column(column =>
                    {
                        column.Item().Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                        if (!_report.IsPublic)
                        {
                            column.Item().PaddingTop(5)
                                .Text("Confidential - Internal Distribution Only")
                                .FontSize(8);
                        }
                    });
            });
        }
    }
}