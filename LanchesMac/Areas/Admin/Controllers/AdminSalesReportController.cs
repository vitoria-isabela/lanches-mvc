using LanchesMac.Areas.Admin.services;
using LanchesMac.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LanchesMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminSalesReportController : Controller
    {
        private readonly SalesReportService _salesReport;

        public AdminSalesReportController(SalesReportService salesReport)
        {
            _salesReport = salesReport ?? throw new ArgumentNullException(nameof(salesReport));
        }

        /// <summary>
        /// Displays the index view.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Generates a simple sales report based on given date range.
        /// </summary>
        /// <param name="minDate">Minimum date for the report.</param>
        /// <param name="maxDate">Maximum date for the report.</param>
        public async Task<IActionResult> SimpleSalesReport(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesReport.FindByDateAsync(minDate, maxDate);
            return View(result);
        }
    }
}
