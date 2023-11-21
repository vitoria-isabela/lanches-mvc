using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        /// <summary>
        /// Displays the Admin dashboard or main page.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }
    }
}