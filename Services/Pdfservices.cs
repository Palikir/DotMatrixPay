using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PayrollPrinterApp.Models;
using System.Collections.Generic;

namespace PayrollPrinterApp.Services
{
    public class PdfService
    {
        public byte[] GeneratePayrollPdf(List<PayrollRecord> records, int startLine, int marginLeft, int marginTop, int lineHeight)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.MarginLeft(marginLeft);
                    page.MarginTop(marginTop);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Courier New"));

                    page.Content().Column(col =>
                    {
                        // Add blank lines for offset
                        for (int i = 0; i < startLine - 1; i++)
                        {
                            col.Item().Text(" ").LineHeight(lineHeight);
                        }

                        foreach (var r in records)
                        {
                            string line = $"{r.SSN,-12}{r.Code1,-6}{r.Code2,-6}{r.LastName,-15}{r.FirstName,-15}{r.FWTax,10}{r.WageTax,10}";
                            col.Item().Text(line).LineHeight(lineHeight);
                        }
                    });
                });
            });

            return document.GeneratePdf();
        }


    }
}
