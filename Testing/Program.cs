using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System.Data;
using Testing;
// This line initializes a WebApplicationBuilder instance, which is responsible for configuring and building the WebApplication.
// The args parameter typically comes from the command-line arguments passed to the application. These can be used for various purposes,
// such as configuring the application. The builder object is now ready to configure services, middleware, and other application settings.
var builder = WebApplication.CreateBuilder(args);

// This line adds support for MVC pattern with views to the service collection (builder.Services).
// The method registers services required for MVC controllers that use views (Razor views).
// It includes support for model binding, validation, and rendering views.
// Overall this line is habdles requests routed to controllers that render views, enabling MVC functionality.
builder.Services.AddControllersWithViews();

// This code sets up a web application with MVC support and configures dependency injection to provide a scoped, open IDbConnection connected to a
// MySQL database. The IDbConnection is made available to other parts of the application, such as controllers or services, ensuring that each
// HTTP request gets its own database connection instance, which is automatically disposed of at the end of the request from the AddScoped method provided
// by the Microsoft.Extensions.DependencyInjection namespace. The (S) in the lambda expression represents IServiceProvider and its services isn't directly
// used within the scope. It's there in case there are DI issues that need to be resolved like logging or configuration.
// This indirect reference of the interface would provide more flexibility for future circumstances if / when they occur.
builder.Services.AddScoped<IDbConnection>((s) =>
{
    IDbConnection conn = new MySqlConnection(builder.Configuration.GetConnectionString("bestbuy"));
    conn.Open();
    return conn;
});
// This line tells the DI system to inject an instance of ProductRepository wherever an IProductRepository is required.
// When the ProductController is instantiated by the framework, it automatically receives an instance of ProductRepository by its constructor.
// This allows the controller to call the GetProduct and GetAllProducts methods on the repository.
builder.Services.AddTransient<IProductRepository, ProductRepository>();

// The Build() method finalizes the settings and locks them in before we use it to configure the HTTP request pipeline.
var app = builder.Build();

// Configure the HTTP request pipeline.
// Checks if the app is not in the "Development" environment. Environment is a property of the WebApplication class which is an instance of
// the IWebHostEnvironment interface.
if (!app.Environment.IsDevelopment())
{
    // Redirects users to a generic error page in case of unhandled exceptions, which is important for user experience and security in production.
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // HSTS (HTTP Strict Transport Security) enforces strict HTTPS only access to the site, enhancing security in production.
    // HTTPS (HyperText Transfer Protocol Secure) is an extension of HTTP (HyperText Transfer Protocol) that provides a secure
    // communication channel over a computer network, typically the internet. It is widely used to secure data exchange between
    // a user's web browser and a web server, ensuring that the data transmitted is encrypted and protected from interception by unauthorized parties.
    app.UseHsts();
}
// This middleware automatically redirects all HTTP requests to their HTTPS equivalents, ensuring that your application is accessed securely.
app.UseHttpsRedirection();

// This enables the serving of static files (such as HTML, CSS, JavaScript, images, and other files) directly to clients.
// This is commonly used to serve the assets that make up the frontend of a web application.
app.UseStaticFiles();

// This enables routing capabilities in the application. It is responsible for matching incoming HTTP requests to the appropriate
// endpoints (such as controllers, actions, or pages) based on the defined routing rules.
app.UseRouting();

// This checks whether authenticated users have the necessary permissions to access specific resources or perform actions,
// thereby enforcing security policies and controlling access within the application.
app.UseAuthorization();

// This method sets up a default routing pattern for the application, mapping incoming requests to the appropriate controller and action method.
// It helps define how URLs are interpreted and directs requests to the correct parts of the application, promoting clean and organized URL structures.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();