using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplicationBPR2.Data.Repository;
using WebApplicationBPR2.Data;
using WebApplicationBPR2.ViewModels;

namespace WebApplicationBPR2.Controllers
{
    public class ShoppingCartController : Controller // of course needed to interface between data model and a cart view
    {
        private readonly IProductRepository _productRepository;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IProductRepository productRepository, ShoppingCart shoppingCart)
        {
            _productRepository = productRepository;
            _shoppingCart = shoppingCart;
        }

        public ViewResult Index()
        {
            var items = _shoppingCart.GetOrderItems(); // requesting for all order items from database
            _shoppingCart.OrderItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel // creating a new viewmodel which contains
            {
                ShoppingCart = _shoppingCart, // the shopping cart
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal() // and total
            };

            return View(shoppingCartViewModel); // and passing this viewmodel to the view
        }


        public RedirectToActionResult AddToShoppingCart(int productId)
        {
            var selectedProduct = _productRepository.Products.FirstOrDefault(p => p.Id == productId);

            if (selectedProduct != null)
            {
                _shoppingCart.AddToCart(selectedProduct, 1);
            }
            //return RedirectToAction("Product", "App"); // returns to the products page, but you can see added prod in the cart
            return RedirectToAction("Index"); // goes to cart

        }

        public RedirectToActionResult RemoveFromShoppingCart(int productId)
        {
            var selectedProduct = _productRepository.Products.FirstOrDefault(p => p.Id == productId);

            if (selectedProduct != null)
            {
                _shoppingCart.RemoveFromCart(selectedProduct);
            }
            return RedirectToAction("Index");
        }

    }
}
