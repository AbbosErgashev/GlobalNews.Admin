using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using News.Admin.Models;

namespace News.Admin.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        try
        {
            _logger.LogInformation("Index action called.");

            var isAuthenticated = HttpContext.Session.GetString("IsAuthenticated");
            if (string.IsNullOrEmpty(isAuthenticated))
            {
                _logger.LogWarning("Unauthenticated access attempt to Index page.");
                return RedirectToAction("Login", "Account");
            }

            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;

            _logger.LogInformation("Authenticated user '{Username}' accessed Index page.", username);
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the Index action.");
            return RedirectToAction("Error");
        }
    }

    public IActionResult Privacy()
    {
        try
        {
            _logger.LogInformation("Privacy action called.");
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the Privacy action.");
            return RedirectToAction("Error");
        }
    }

    /// <summary>
    /// Exception Handling
    /// </summary>
    /// <returns></returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        _logger.LogInformation("Error action called. RequestId: {RequestId}", requestId);
        return View(new ErrorViewModel { RequestId = requestId });
    }
}
