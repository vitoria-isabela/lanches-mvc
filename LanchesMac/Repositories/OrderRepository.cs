using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using System;

namespace LanchesMac.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ShoppingCart _shoppingCart;

        public OrderRepository(AppDbContext appDbContext, ShoppingCart shoppingCart)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
        }

        public void CreateOrder(Order order)
        {
            order.OrderSent = DateTime.Now;

            _appDbContext.Orders.Add(order);
            _appDbContext.SaveChanges();

            var buyItemsCart = _shoppingCart.BuyItemsCart;

            foreach (var cartItem in buyItemsCart)
            {
                var orderDetail = new OrderDetail()
                {
                    Quantity = cartItem.Quantity,
                    SnackId = cartItem.Snack.SnackId,
                    OrderId = order.OrderId,
                    Price = cartItem.Snack.Price
                };
                _appDbContext.OrderDetails.Add(orderDetail);
            }
            _appDbContext.SaveChanges();
        }
    }
}
