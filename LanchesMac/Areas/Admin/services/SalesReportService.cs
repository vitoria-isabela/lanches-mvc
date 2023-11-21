using LanchesMac.Context;
using LanchesMac.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.Areas.Admin.Services
{
    /// <summary>
    /// Service handling sales report-related operations.
    /// </summary>
    public class SalesReportService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesReportService"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public SalesReportService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Finds orders within a specified date range asynchronously.
        /// </summary>
        /// <param name="minDate">The minimum date for filtering orders.</param>
        /// <param name="maxDate">The maximum date for filtering orders.</param
