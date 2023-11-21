using LanchesMac.Areas.Admin.services;
using LanchesMac.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminGraphicController : Controller
    {
        private readonly SalesGraphicService _graphic;

        public AdminGraphicController(SalesGraphicService graphic)
        {
            _graphic = graphic ?? throw new ArgumentNullException(nameof(graphic));
        }

        /// <summary>
        /// Retrieves snack sales data for a specific number of days.
        /// </summary>
        /// <param name="days">Number of days to retrieve sales data for.</param>
        public JsonResult SnacksSales(int days)
        {
            var snacksSalesTotal = _graphic.GetSnackSales(days);
            return Json(snacksSalesTotal);
        }

        /// <summary>
        /// Displays the index view.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the view for monthly sales.
        /// </summary>
        [HttpGet]
        public IActionResult MonthlySales()
        {
            return View();
        }

        /// <summary>
        /// Displays the view for weekly sales.
        /// </summary>
        [HttpGet]
        public IActionResult WeeklySales()
        {
            return View();
        }
    }
}
