using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Testing.Models;

namespace Testing.Controllers
{
    public class ProductController : Controller
    {
        // Allows the ProductController class to use IProductRepository's methods that were implemented in
        // the ProductRepository class throughout the ProductController class which is called dependency injection (DI).
        // private access modifier indicates it can only be accessed within the ProductController class.
        // readonly ensures that repo cannot be modified after the object is constructed.
        private readonly IProductRepository repo;

        // Constructor for ProductController class assigning some DI from the IProductRepository class to it.
        // The methods that reside in ProductRepository class can't be implemented until it's instantiated in the program class.
        public ProductController(IProductRepository repo)
        {
            this.repo = repo;
        }

        // IActionResult is a common return type in ASP.NET Core controllers.
        // It allows the method to return various types of HTTP responses, such as views, redirects, or JSON data.
        // This method retrieves a list of all products then returns it to the View() method.
        public IActionResult Index()
        {
            var products = repo.GetAllProducts();
            return View(products);
        }
        // Retrieves a single product to the view method.
        public IActionResult ViewProduct(int id)
        {
            var product = repo.GetProduct(id);
            return View(product);
        }
        // Retrieves a product id if it exists and returns the view of its details by passing the prod object to the view.
        public IActionResult UpdateProduct(int id)
        {
            Product prod = repo.GetProduct(id);

            if(prod == null)
            {
                return View("ProductionNotFound");
            }
            return View(prod);
        }
        // Calls the UpdateProduct method to update product information in the database with the data from the product object
        // passed to this method.
        // Redirect to ViewProduct: Redirects the user to the ViewProduct Razor method after updating the product
        // then passes the ProductID of the updated product as a parameter. This shows the updated details of the product.
        public IActionResult UpdateProductToDatabase(Product product)
        {
            repo.UpdateProduct(product);
            return RedirectToAction("ViewProduct", new { id = product.ProductID });
        }
        public IActionResult InsertProduct()
        {
            var prod = repo.AssignCategory();
            return View(prod);
        }
        public IActionResult InsertProductToDatabase(Product productToInsert)
        {
            repo.InsertProduct(productToInsert);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteProduct(Product product)
        {
            repo.DeleteProduct(product);
            return RedirectToAction("Index");
        }
    }
}
