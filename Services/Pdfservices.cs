using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PayrollPrinterApp.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Data;

namespace PayrollPrinterApp.Services
{
    public class PdfService
    {

        public byte[] GeneratePayrollPdf(List<PayrollRecord> records, int startLine, PrintSettings settings, string state)
        {
            int widthPoints = (int)(settings.PageWidthInches * 72);
            int heightPoints = (int)(settings.PageHeightInches * 72);

            var document = Document.Create(container =>
            {
                foreach (var r in records.Skip(startLine - 1))
                {
                    container.Page(page =>
                    {
                        page.Size(new PageSize(widthPoints, heightPoints));
                        page.MarginLeft(settings.MarginLeft);
                        page.MarginTop(settings.MarginTop-34);
                        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Times New Roman"));

                        page.Content().Column(col =>
                        {
                            // Employer Tax ID, Name, Tax Year
                            col.Item().Row(row=>
                            {
                                row.RelativeColumn(2).Text($"{settings.EmployerTaxId}");
                                row.RelativeColumn(1).Text($"{settings.EmployerName}");
                                row.RelativeColumn(1).Text(" ");
                                row.RelativeColumn(1).Text(DateTime.Now.Year - 1);
                                // row.RelativeColumn(2).Column(inner=>
                                // {
                                //    inner.Item().Text($"{settings.EmployerName}"); 
                                //    inner.Item().Text(DateTime.Now.Year - 1); 
                                //    //inner.Item().RelativeColumn().Text(DateTime.Now.Year - 1);
                                // //    inner.Item().Row(innerRow=>{
                                // //         innerRow.RelativeColumn().Text(DateTime.Now.Year - 1);
                                // //     });
                                // });
                            });
                            // Just for COLLEGE word/line
                            col.Item().Row(row=>
                            {
                                row.RelativeColumn(2).Text("");
                                row.RelativeColumn(1).Text("COLLEGE");
                                row.RelativeColumn(1).Text("");
                                row.RelativeColumn(1).Text("");
                            });
                            col.Item().Text("");
                            col.Item().Row(row=>
                            {
                                row.RelativeColumn(1).Text($"{settings.EmployerAddress}");
                                row.RelativeColumn(1).Text("KOLONIA");
                                row.RelativeColumn(1).Text("POHNPEI");
                                row.RelativeColumn(1).Text("96941");
                            });
                            col.Item().Text(" "); // blank line
                            col.Item().Row(row =>
                            {
                                row.RelativeColumn(1).Text($"{r.SSN}");
                                row.RelativeColumn(1).Text($"{r.SSTax}");
                                if (state == "Yap")
                                {                                    
                                    row.RelativeColumn(1).Text("");
                                    row.RelativeColumn(1).Text($"{r.WageTax}");
                                    row.RelativeColumn(1).Text($"{r.FWTax}");
                                }
                                else
                                {                                    
                                    row.RelativeColumn(1).Text("");
                                    row.RelativeColumn(1).Text("");
                                    row.RelativeColumn(1).Text("");
                                }
                            });

                            col.Item().Text(" "); // blank line
                            if (state=="Chuuk")
                            {
                                 col.Item().Row(row =>
                                 {
                                    row.RelativeColumn(1).Text("");
                                    row.RelativeColumn(1).Text($"{r.WageTax}");
                                    row.RelativeColumn(1).Text($"{r.FWTax}");                                
                                 });
                                col.Item().Text(" "); // blank line
                            }
                            else
                            {
                                col.Item().Text(" "); // blank line
                                col.Item().Text(" "); // blank line
                                col.Item().Text(" "); // blank line
                            }

                            // Employee details in columns
                            if (state=="Pohnpei")
                            {
                                 col.Item().Text(" "); // blank line
                                 col.Item().Row(row =>
                                 {
                                    row.RelativeColumn(2).Text($"{r.LastName}, {r.FirstName}");
                                    row.RelativeColumn(1).Text(" ");
                                    row.RelativeColumn(1).Text($"{r.WageTax}");
                                    row.RelativeColumn(1).Text($"{r.FWTax}");                                
                                 });
                                
                            }
                            else
                            {
                                col.Item().Text($"{r.LastName}, {r.FirstName}"); 
                            }
                            col.Item().Text(" "); // blank line
                            if (state == "Kosrae")
                            {
                                 col.Item().Row(row =>
                                 {
                                    row.RelativeColumn(2).Text("C/O P.O. Box 159");
                                    row.RelativeColumn(1).Text(" ");
                                    row.RelativeColumn(1).Text($"{r.WageTax}");
                                    row.RelativeColumn(1).Text($"{r.FWTax}");                                
                                 });
                                
                            }
                            else
                            {                           
                                col.Item().Text("C/O P.O. Box 159");
                            }
                            col.Item().Text(" "); // blank line
                            col.Item().Row(row=>
                            {
                                row.RelativeColumn(2).Text("KOLONIA, POHNPEI, FSM");
                                row.RelativeColumn(1).Text("96941");
                                row.RelativeColumn(1).Text($"{r.WageTax}");
                                row.RelativeColumn(1).Text($"{r.FWTax}");
                            });


                            // col.Item().Row(row =>
                            // {
                            //     row.RelativeColumn().Text($"Employee: {r.LastName}, {r.FirstName}");
                            //     row.RelativeColumn().Text($"SSN: {r.SSN}");
                            // });

                            // col.Item().Row(row =>
                            // {
                            //     row.RelativeColumn().Text($"Wages: {r.WageTax}");
                            //     row.RelativeColumn().Text($"FWTax: {r.FWTax}");
                            // });

                            // col.Item().Row(row =>
                            // {
                            //     row.RelativeColumn().Text($"Code1: {r.Code1}");
                            //     row.RelativeColumn().Text($"Code2: {r.Code2}");
                            // });
                        });
                    });
                }
            });

            return document.GeneratePdf();
        }


        
    }
}
