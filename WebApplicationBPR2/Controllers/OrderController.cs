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
using WebApplicationBPR2.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationBPR2.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly IMailService _mailService;

        public OrderController(IOrderRepository orderRepository, ShoppingCart shoppingCart, IMailService mailService)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _mailService = mailService;
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
                // send email to the customer service departm 
                _mailService.SendMail("adrianraspopa@gmail.com", "New Order", $"From: {order.Email} ; Message: You've got a new order; Order Details: " +
                    $"FirstName: {order.FirstName}, " +
                    $"LastName: {order.LastName}, " +
                    $"AddressLine1: {order.AddressLine1}, " +
                    $"AddressLine2: {order.AddressLine2}, " +
                    $"Items: {order.Items}, " +
                    $"OrderTotal: {order.OrderTotal}, " +
                    $"Phone: {order.PhoneNumber}, " +
                    $"Country: {order.Country}, " +
                    $"Zip: {order.ZipCode}"
                    );
                //and a copy to the client as confirmation
                _mailService.SendMail($"{order.Email}", "Your order has been received", "From: sales@freeze.com; Message: WHATEVER DEFAULT MESSAGE WE HAVE FOR THE CLIENT");
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            return View();
        }
    }
}
