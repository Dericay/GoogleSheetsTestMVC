using GoogleSheetsTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoogleSheetsTest.Controllers
{
    public class SheetsController : Controller
    {
        private readonly GoogleSheetsService _googleSheetsService;

        public SheetsController(GoogleSheetsService googleSheetsService)
        {
            _googleSheetsService = googleSheetsService;
        }

        [HttpGet]
        public IActionResult GetSheetData()
        {
            var range = "Sheet1!A1:N20";
            var data = _googleSheetsService.ReadEntries(range);

            if (data != null && data.Count > 0)
            {
                return View(data);
            }
            else
            {
                return NotFound("No data found.");
            }
        }
    }
}
