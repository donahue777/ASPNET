using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Testing.Controllers
{
    public class ProductController : Controller
    {
        // Allows the ProductController class to use IProductRepository's methods that were implemented in
        // the ProductRepository class throughout the ProductController class which is called dependency injection (DI).
        // Dependency Injection was used in the Program class as this; builder.Services.AddTransient<IProductRepository, ProductRepository>();
        // private access modifier indicates it can only be accessed within the ProductController class.
        // readonly ensures that repo cannot be modified after the object is constructed.
        private readonly IProductRepository repo;

        public ProductController(IProductRepository repo)
        {
            this.repo = repo;
        }

        // IActionResult is a common return type in ASP.NET Core controllers.
        // It allows the method to return various types of HTTP responses, such as views, redirects, or JSON data.
        public IActionResult Index()
        {
            var products = repo.GetAllProducts();
            return View(products);
        }
        public IActionResult ViewProduct(int id)
        {
            var product = repo.GetProduct(id);
            return View(product);
        }
    }
}
