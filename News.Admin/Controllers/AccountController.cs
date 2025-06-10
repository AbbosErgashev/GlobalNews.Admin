using Microsoft.AspNetCore.Mvc;

namespace News.Admin.Controllers;

public class AccountController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IConfiguration configuration, ILogger<AccountController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    /// <summary>
    /// Login/Password GET
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult Login()
    {
        _logger.LogInformation("Login page requested");
        return View();
    }

    /// <summary>
    /// Login/Password POST
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        try
        {
            _logger.LogInformation("Login attempt for username: {Username}", username);

            var configUsername = _configuration["Credentials:Username"];
            var configPassword = _configuration["Credentials:Password"];

            if (string.IsNullOrEmpty(configUsername) || string.IsNullOrEmpty(configPassword))
            {
                _logger.LogError("Configuration credentials are not set properly");
                throw new ApplicationException("Server configuration error");
            }

            if (username == configUsername && password == configPassword)
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("Username", username);

                _logger.LogInformation("Successful login for username: {Username}", username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogWarning("Failed login attempt for username: {Username}", username);
                ViewBag.Error = "Invalid username or password";
                return View();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during login attempt for username: {Username}", username);
            ViewBag.Error = "An error occurred during login. Please try again.";
            return View();
        }
    }

    public IActionResult Logout()
    {
        try
        {
            var username = HttpContext.Session.GetString("Username") ?? "unknown";
            HttpContext.Session.Clear();

            _logger.LogInformation("User {Username} logged out successfully", username);
            return RedirectToAction("Login", "Account");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during logout");
            return RedirectToAction("Login", "Account");
        }
    }
}
