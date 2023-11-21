using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LanchesMac.Controllers
{
    /// <summary>
    /// Controller responsible for managing snack-related actions.
    /// </summary>
    public class SnackController : Controller
    {
        private readonly ISnackRepository _snackRepository;

        public SnackController(ISnackRepository snackRepository)
        {
            _snackRepository = snackRepository;
        }

        /// <summary>
        /// Lists snacks based on the specified category or displays all snacks if no category is specified.
        /// </summary>
        public IActionResult List(string category)
        {
            IEnumerable<Snack> snacks;
            string currentCategory = string.Empty;

            if (string.IsNullOrEmpty(category))
            {
                snacks = _snackRepository.Snacks.OrderBy(l => l.SnackId);
                currentCategory = "All snacks";
            }
            else
            {
                snacks = _snackRepository.Snacks
                    .Where(s => s.Category.CategoryName.Equals(category))
                    .OrderBy(c => c.Name);
                currentCategory = category;
            }

            var snackListViewModel = new SnackListViewModel
            {
                Snacks = snacks,
                CurrentCategory = currentCategory
            };

            return View(snackListViewModel);
        }

        /// <summary>
        /// Displays details of a specific snack.
        /// </summary>
        public IActionResult Details(int snackId)
        {
            var snack = _snackRepository.Snacks.FirstOrDefault(s => s.SnackId == snackId);
            return View(snack);
        }

        /// <summary>
        /// Searches for snacks based on the provided search string.
        /// </summary>
        public IActionResult Search(string searchString)
        {
            IEnumerable<Snack> snacks;
            string currentCategory = string.Empty;

            if (string.IsNullOrEmpty(searchString))
            {
                snacks = _snackRepository.Snacks.OrderBy(s => s.SnackId);
                currentCategory = "All snacks";
            }
            else
            {
                snacks = _snackRepository.Snacks
                    .Where(s => s.Name.ToLower().Contains(searchString.ToLower())
                    || s.ShortDescription.ToLower().Contains(searchString.ToLower()));

                currentCategory = snacks.Any() ? $"All results found by {searchString}" : $"No snacks were found by {searchString}";
            }

            return View("~/Views/Snack/List.cshtml", new SnackListViewModel
            {
                Snacks = snacks,
                CurrentCategory = currentCategory
            });
        }
    }
}
