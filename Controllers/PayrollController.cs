using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PayrollPrinterApp.Models;
using PayrollPrinterApp.Services;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace PayrollPrinterApp.Controllers
{
    public class PayrollController : Controller
    {
        private readonly PdfService _pdfService = new PdfService();
        private readonly IConfiguration _config; 
        
        public PayrollController(IConfiguration config) { _config = config; }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file, string state, int startLine = 1)
        {
            if (file == null || file.Length == 0)
                return Content("No file selected");

            var records = new List<PayrollRecord>();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = startLine + 1; row <= rowCount; row++) // skip header
                    {
                        records.Add(new PayrollRecord
                        {
                            SSN = worksheet.Cells[row, 1].Text,
                            Code1 = worksheet.Cells[row, 2].Text,
                            Code2 = worksheet.Cells[row, 3].Text,
                            LastName = worksheet.Cells[row, 4].Text,
                            FirstName = worksheet.Cells[row, 5].Text,
                            FWTax = decimal.TryParse(worksheet.Cells[row, 6].Text, out var fw) ? fw : 0,
                            WageTax = decimal.TryParse(worksheet.Cells[row, 7].Text, out var wt) ? wt : 0,
                            SSTax = worksheet.Cells[row, 8].Text
                            // SSTax = decimal.TryParse(worksheet.Cells[row, 8].Text, out var st) ? st : 0
                        });
                    }
                }
            }

            var settings = SettingsController.GetSettings();
            var pdfBytes = _pdfService.GeneratePayrollPdf(records, startLine, settings, state);

            // Preview mode: inline display
            return File(pdfBytes, "application/pdf");
        }

        public IActionResult Upload() 
        { 
            int marginLeft = _config.GetValue<int>("PrintSettings:MarginLeft"); 
            int marginTop = _config.GetValue<int>("PrintSettings:MarginTop"); 
            int lineHeight = _config.GetValue<int>("PrintSettings:LineHeight"); // You can pass these values to the view 
        
            ViewBag.MarginLeft = marginLeft; 
            ViewBag.MarginTop = marginTop; 
            ViewBag.LineHeight = lineHeight; 
        
            return View(); 
        }

    }
}
