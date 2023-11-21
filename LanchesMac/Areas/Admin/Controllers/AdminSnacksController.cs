using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LanchesMac.Context;
using LanchesMac.Models;
using ReflectionIT.Mvc.Paging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace LanchesMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminSnacksController : Controller
    {
        private readonly AppDbContext _context;

        public AdminSnacksController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a paginated list of snacks with optional filtering and sorting.
        /// </summary>
        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Snacks.Include(s => s.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(s => s.Name.Contains(filter));
            }

            var model = await PagingList.CreateAsync(result, 5, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return View(model);
        }

        // Other actions (Details, Create, Edit, Delete) remain the same

        // ... [rest of the code] ...

        private bool SnackExists(int id)
        {
            return _context.Snacks.Any(e => e.SnackId == id);
        }
    }
}
