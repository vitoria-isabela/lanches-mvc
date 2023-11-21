﻿using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LanchesMac.Controllers
{
    /// <summary>
    /// Controller responsible for handling home-related views and actions.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ISnackRepository _snackRepository;

        public HomeController(ISnackRepository snackRepository)
        {
            _snackRepository = snackRepository;
        }

        /// <summary>
        /// Displays the home page with a list of favorite snacks.
        /// </summary>
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                FavoriteSnacks = _snackRepository.FavoriteSnacks
            };
            return View(homeViewModel);
        }

        /// <summary>
        /// Displays the error page.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
