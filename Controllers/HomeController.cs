// Lisha Naidoo
// ST10404816
// Group 1

// References
// https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
// https://dotnethow.net/a-step-by-step-guide-to-configuring-entity-framework-in-your-net-web-api-project/

using Microsoft.AspNetCore.Mvc;
using POE.Models;
using System.Diagnostics;

namespace POE.Controllers
{
    public class HomeController : Controller
    {

        // Injects an ILogger instance to log information, warnings, or errors.
        private readonly ILogger<HomeController> _logger;

        //------------------------------------------------------------------------------------------------------------------------//
        // Constructor that initializes the logger via Dependency Injection.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;  // Assign the injected logger to a private field.
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // Handles requests to the home (index) page.
        public IActionResult Index()
        {
            // Return the "Index" view, which corresponds to the default home page.
            return View();
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // Displays the error page.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Creates and returns the "Error" view with an ErrorViewModel.
            // If the current Activity or RequestId is null, it uses HttpContext.TraceIdentifier instead.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//------------------------------------------...ooo000 END OF FILE 000ooo...------------------------------------------------------//
