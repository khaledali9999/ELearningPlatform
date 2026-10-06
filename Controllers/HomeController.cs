using Microsoft.AspNetCore.Mvc;

namespace ELearningPlatform.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
