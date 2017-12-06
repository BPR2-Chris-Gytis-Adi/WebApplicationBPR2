using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplicationBPR2.Data.Repository;
using WebApplicationBPR2.Data;
using Microsoft.AspNetCore.Authorization;
using WebApplicationBPR2.Data.Entities;
using Stripe;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationBPR2.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;

        public OrderController(IOrderRepository orderRepository, ShoppingCart shoppingCart)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
        }


        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Checkout(Order order)
        {
            var items = _shoppingCart.GetOrderItems();
            _shoppingCart.OrderItems = items;
            if (_shoppingCart.OrderItems.Count == 0)
            {
                ModelState.AddModelError("", "Your card is empty, add some products first");
            }

            if (ModelState.IsValid)
            {
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            return View(order);
        }

        public IActionResult CheckoutComplete(string stripeEmail, string stripeToken)
        {
            var customerService = new StripeCustomerService();
            var chargeService = new StripeChargeService();

            var customer = customerService.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            try
            {
                var charge = chargeService.Create(new StripeChargeCreateOptions
                {
                    CustomerId = customer.Id,
                    Amount = (int)_shoppingCart.GetShoppingCartTotal() * 100,
                    Currency = "usd"
                });
            }
            catch (Exception)
            {

                throw;
            }
            

            return View();
        }
    }
}
