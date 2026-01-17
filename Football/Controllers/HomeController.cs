using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

namespace Football.Controllers
{
    public class HomeController : Controller
    {
        // Sadə landing / home page
        public IActionResult Index()
        {
            return View();
        }

        // Error page (istəsən genişləndirərik)
        public IActionResult Error()
        {
            return View();
        }
    }
}
