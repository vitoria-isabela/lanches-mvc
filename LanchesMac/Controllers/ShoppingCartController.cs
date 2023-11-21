﻿using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    /// <summary>
    /// Controller responsible for managing the shopping cart functionality.
    /// </summary>
    public class ShoppingCartController : Controller
    {
        private readonly ISnackRepository _snackRepository;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(ISnackRepository snackRepository, ShoppingCart shoppingCart)
        {
            _snackRepository = snackRepository;
            _shoppingCart = shoppingCart;
        }

        /// <summary>
        /// Displays the shopping cart items and their total.
        /// </summary>
        [Authorize]
        public IActionResult Index()
        {
            var items = _shoppingCart.GetBuyItemsCart();
            _shoppingCart.BuyItemsCart = items;

            var buyCartVM = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetBuyCartTotal()
            };
            return View(buyCartVM);
        }

        /// <summary>
        /// Adds a selected snack item to the shopping cart.
        /// </summary>
        [Authorize]
        public IActionResult AddItemToCart(int snackId)
        {
            var selectedSnack = _snackRepository.Snacks.FirstOrDefault(x => x.SnackId == snackId);
            if (selectedSnack != null)
            {
                _shoppingCart.AddToCart(selectedSnack);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Removes a selected snack item from the shopping cart.
        /// </summary>
        [Authorize]
        public IActionResult RemoveItemFromCart(int snackId)
        {
            var selectedSnack = _snackRepository.Snacks.FirstOrDefault(x => x.SnackId == snackId);
            if (selectedSnack != null)
            {
                _shoppingCart.RemoveFromCart(selectedSnack);
            }
            return RedirectToAction("Index");
        }
    }
}
