using Microsoft.AspNetCore.Mvc;

namespace News.Admin.Controllers;

public class AccountController : Controller
{
    private readonly IConfiguration _configuration;

    public AccountController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var configUsername = _configuration["Credentials:Username"];
        var configPassword = _configuration["Credentials:Password"];

        if (username == configUsername && password == configPassword)
        {
            HttpContext.Session.SetString("IsAuthenticated", "true");
            HttpContext.Session.SetString("Username", username);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.Error = "Login yoki parol noto‘g‘ri";
            return View();
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }
}
