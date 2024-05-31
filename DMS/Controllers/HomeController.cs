using System.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DMS.Models;
using MongoDB.Driver;

namespace DMS.Controllers;

public class HomeController : Controller
{
    private IConfiguration _configuration;  
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger,IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        string connectionString = _configuration.GetConnectionString("MongoDBDmsDB");
        
        if (connectionString == null)
        {
            Console.WriteLine("You must set your 'MONGODB_URI' environment variable. To learn how to set it, see https://www.mongodb.com/docs/drivers/csharp/current/quick-start/#set-your-connection-string");
            Environment.Exit(0);
        }
        var client = new MongoClient(connectionString);
        var db = DmsDbContext.Create(client.GetDatabase("DMSDB"));
        var dev = db.Developers.Count();

        ViewData["devcount"] = dev;
            
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}