using LanchesMac.Context;
using LanchesMac.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanchesMac.Areas.Admin.Services
{
    /// <summary>
    /// Service handling sales graphic-related operations.
    /// </summary>
    public class SalesGraphicService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesGraphicService"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public SalesGraphicService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves sales information for snacks within a specific time range.
        /// </summary>
        /// <param name="days">The number of days for the sales information (default is 360 days).</param>
        /// <returns>A list of snack graphics containing sales data.</returns>
        public List<SnackGraphic> GetSnackSales(int days = 360)
        {
            var date = DateTime.Now.AddDays(-days);

            var snacksQuery = from od in _context.OrderDetails
                              join s in _context.Snacks on od.SnackId equals s.SnackId
                              where od.Order.OrderSent >= date
                              group od by new { od.SnackId, s.Name } into g
                              select new
                              {
                                  SnackName = g.Key.Name,
                                  SnacksQuantity = g.Sum(q => q.Quantity),
                                  SnacksTotalValue = g.Sum(t => t.Price * t.Quantity)
                              };

            var snackGraphics = snacksQuery.Select(item => new SnackGraphic
            {
                SnackName = item.SnackName,
                SnacksQuantity = item.SnacksQuantity,
                SnacksTotalValue = item.SnacksTotalValue
            }).ToList();

            return snackGraphics;
        }
    }
}
