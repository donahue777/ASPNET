using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Testing.Models;
using Dapper;

namespace Testing
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _conn;

        public ProductRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _conn.Query<Product>("SELECT * FROM PRODUCTS;"); 
        }
        // The @id in the SQL statement is a parameterized placeholder indicating that the actual value will be supplied at runtime.
        // Using parameterized queries is crucial for preventing SQL injection attacks. The actual value for @id is provided
        // separately which enhances security. The value for @id is implemented in the C% code; new{ id = id}); which is an anonymous
        // object tool used within the method to bundle the parameters together. An anonymous object in C# is an object without
        // explicitly defining a class for it. Instead, you define the properties of the object directly when you create it.
        // The first id is the property name in the anonymous object. The second id is the method parameter that was passed into
        // GetProduct(int id). The anonymous object is used to map the id property to the id parameter in the SQL query which then
        // selects the specific value of product id within SQL. _conn is an instance of IDbConnection which represents an open
        // connection to a database. QuerySingle<Product> is a method provided by Dapper in .NET. This method executes the SQL
        // query and expects a single row to be returned which it maps to the product object which is then returned to the
        // GetProduct method.
        public Product GetProduct(int id)
        {
            return _conn.QuerySingle<Product>("SELECT * FROM PRODUCTS WHERE PRODUCTID = @id", new {id = id});
        }
        public void UpdateProduct(Product product)
        {
            _conn.Execute("UPDATE products SET Name = @name, Price = @price WHERE ProductID = @id",
                new { name = product.Name, price = product.Price, id = product.ProductID });
        }
        public void InsertProduct(Product productToInsert)
        {
            _conn.Execute("INSERT INTO products (NAME, PRICE, CATEGORYID) VALUES (@name, @price, @categoryID);",
                new { name = productToInsert.Name, price = productToInsert.Price, categoryID = productToInsert.CategoryID });
        }
        public IEnumerable<Category> GetCategories()
        {
            return _conn.Query<Category>("SELECT * FROM categories;");
        }
        public Product AssignCategory()
        {
            var categoryList = GetCategories();
            var product = new Product();
            product.Categories = categoryList;
            return product;
        }
        public void DeleteProduct(Product product)
        {
            _conn.Execute("DELETE FROM REVIEWS WHERE ProductID = @id;", new { id = product.ProductID });
            _conn.Execute("DELETE FROM Sales WHERE ProductID = @id;", new { id = product.ProductID });
            _conn.Execute("DELETE FROM Products WHERE ProductID = @id;", new { id = product.ProductID });
        }
    }
}
