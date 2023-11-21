using LanchesMac.Context;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : Controller
    {
        private readonly AppDbContext _context;

        public AdminOrdersController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the ordered snacks for a specific order.
        /// </summary>
        public IActionResult OrderSnacks(int? id)
        {
            var order = _context.Orders
                .Include(od => od.OrderItems)
                .ThenInclude(s => s.Snack)
                .FirstOrDefault(p => p.OrderId == id);

            if (order == null)
            {
                Response.StatusCode = 404;
                return View("OrderNotFound", id.Value);
            }

            var orderSnacks = new OrderSnackViewModel()
            {
                Order = order,
                orderDetails = order.OrderItems
            };

            return View(orderSnacks);
        }

        /// <summary>
        /// Displays a paginated list of orders with optional filtering and sorting.
        /// </summary>
        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Orders.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => p.Name.Contains(filter));
            }

            var model = await PagingList.CreateAsync(result, 5, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return View(model);
        }

        // Other actions (Create, Edit, Details, Delete) remain the same

        // ... [rest of the code] ...

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
