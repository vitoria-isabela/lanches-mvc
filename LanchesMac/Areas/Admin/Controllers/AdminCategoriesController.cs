using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LanchesMac.Context;
using LanchesMac.Models;
using Microsoft.AspNetCore.Authorization;

namespace LanchesMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public AdminCategoriesController(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        /// <summary>
        /// Retrieves details of a category by ID.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        /// <summary>
        /// Displays the form to create a new category.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        /// <summary>
        /// Displays the form to edit a category by ID.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        /// <summary>
        /// Edits a category.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description")] Category category)
        {
            if (id != category.CategoryId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                        return NotFound();

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        /// <summary>
        /// Displays the form to confirm deletion of a category by ID.
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}