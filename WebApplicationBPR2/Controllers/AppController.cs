using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationBPR2.Data;
using WebApplicationBPR2.Data.Repository;
using WebApplicationBPR2.Services;
using WebApplicationBPR2.ViewModels;

namespace WebApplicationBPR2.Controllers
{
    // a cuontroller allows us to map a request that comes in to a specific action
    public class AppController : Controller 
    {
        private readonly IMailService _mailService;
        private readonly IDataRepository _dataRepository;
        private readonly IProductRepository _productRepository;

        public AppController(IMailService mailService, IDataRepository dataRepository, IProductRepository productRepository)
        {
            _mailService = mailService;
            _dataRepository = dataRepository;
            _productRepository = productRepository;
        }
        //displays by default a view called Index from path Views/(folder with the name of our controller("App"))
        public IActionResult Index() 
        {
            return View();
        }

        // delievers a request; // routing attribute so that we can make contact page more discoverable. Now we can ask specifically for website.com/contact not for the whole path website.com/app/contact
        // we can add the name displayed on the tab of browser from the controller instead of setting it in the view
        //displays by default a view called Index from path Views/(folder with the name of our controller("App"))
        [HttpGet("contact")]    
        public IActionResult Contact() 
        {
            ViewBag.Title = "Contact Us"; 
            return View(); 
        }

        // for receiving user info
        // MVC will map the names of the inputs/area fields in the contact form to the properties in ContactViewModel;
        // When model binding happens, it also affects a ModelState property, so for validation we can check if ModelState is valid when the data comes in 
        //displays by default a view called Index from path Views/(folder with the name of our controller("App"))
        [HttpPost("contact")]       
        public IActionResult Contact(ContactViewModel contactViewModel) 
            
        {
            if(ModelState.IsValid)
            {
                // send email to the customer service departm 
                _mailService.SendMail("adrianraspopa@gmail.com", contactViewModel.Subject, $"From: {contactViewModel.Name} - {contactViewModel.Email}; Message: {contactViewModel.Message}");
                //and a copy to the client as confirmation
                _mailService.SendMail($"{contactViewModel.Email}", contactViewModel.Subject, $"From: {contactViewModel.Name} - {contactViewModel.Email}; Message: {contactViewModel.Message}");
                ViewBag.UserMessage = "Mail sent successfully";
                ModelState.Clear();
            }
            else
            {
                // show errors
            }
            return View(); 
        }

        //displays by default a view called Index from path Views/(folder with the name of our controller("App"))
        [HttpGet("about")]
        public IActionResult About() //an action
        {
            ViewBag.Title = "About Us";
            return View(); 
        }

        [HttpGet("products")]
        public IActionResult Product() //an action
        {
            ViewBag.Title = "Our Products";

            //get the products though the _dataContext object we injected in the constructor
            var products = _dataRepository.GetAllProducts();
            return View(products); // passing data to the view
        }

        [HttpGet("blog")]
        public IActionResult Blog() //an action
        {
            ViewBag.Title = "Blog";
            return View();
        }

        [HttpGet("shop")]
        public IActionResult Order() //an action
        {
            ViewBag.Title = "Shop";
            //get the products though the _dataContext object we injected in the constructor
            var products = _dataRepository.GetAllProducts();
            return View(products); // passing data to the view
        }

        
        public IActionResult SinglePost() //an action
        {
            ViewBag.Title = "Blog Articles";
            return View();
        }



        // categories of products
        public IActionResult IceCream() //an action
        {
            ViewBag.Title = "Ice Cream";
            var products = _dataRepository.GetProductsByCategory("Ice Cream");
            return View(products);
        }

        public IActionResult Cakes() //an action
        {
            ViewBag.Title = "Cakes";
            var products = _dataRepository.GetProductsByCategory("Cakes");
            return View(products);
        }

        public IActionResult Tarts() //an action
        {
            ViewBag.Title = "Tarts";
            var products = _dataRepository.GetProductsByCategory("Tarts");
            return View(products);
        }
    }
}
