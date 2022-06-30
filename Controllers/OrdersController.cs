using eTickets.Data.Cart;
using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IMoviesServices _moviesServices;
        private readonly ShoppingCart _shoppinCart;
        private readonly IOrdersService _ordersService;
        public OrdersController(IMoviesServices moviesServices, ShoppingCart shoppinCart, IOrdersService ordersService)
        {
            _moviesServices = moviesServices;
            _shoppinCart = shoppinCart;
            _ordersService = ordersService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId,userRole);
            return View(orders);
        }
        public IActionResult ShoppingCart()
        {
            var items = _shoppinCart.GetShoppingCartItems();

            _shoppinCart.ShoppingCartItems = items;

            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppinCart,
                ShoppingCartTotal = _shoppinCart.GetShoppingCartTotal()
            };
            return View(response);
        }

        public async Task<RedirectToActionResult> AddToShoppingCart(int id)
        {
            var movie = await _moviesServices.GetMovieByIdAsync(id);

            if(movie != null)
            {
                _shoppinCart.AddItemToCart(movie);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }
        public async Task<IActionResult> RemoveItemFromCart(int id)
        {
            var movie = await _moviesServices.GetMovieByIdAsync(id);

            if (movie != null)
            {
                _shoppinCart.RemoveItemFromCart(movie);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppinCart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmail = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, userId, userEmail);
            await _shoppinCart.ClearShoppingCartAsync();
            return View("OrderCompleted");
        }
    }
}
