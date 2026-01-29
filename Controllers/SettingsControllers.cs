using Microsoft.AspNetCore.Mvc;
using PayrollPrinterApp.Models;

namespace PayrollPrinterApp.Controllers
{
    public class SettingsController : Controller
    {
        // For simplicity, store settings in memory (static).
        // Later you can persist to a database or config file.
        private static PrintSettings _settings = new PrintSettings();

        [HttpGet]
        public IActionResult Index()
        {
            return View(_settings);
        }

        [HttpPost]
        public IActionResult Save(PrintSettings settings)
        {
            _settings = settings; // overwrite with new values
            TempData["Message"] = "Settings saved successfully!";
            return RedirectToAction("Index");
        }

        // Helper method for other controllers
        public static PrintSettings GetSettings()
        {
            return _settings;
        }
    }
}
