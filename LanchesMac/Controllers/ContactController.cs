using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    /// <summary>
    /// Controller for handling contact-related views and actions.
    /// </summary>
    public class ContactController : Controller
    {
        /// <summary>
        /// Displays the contact page.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }
    }
}
