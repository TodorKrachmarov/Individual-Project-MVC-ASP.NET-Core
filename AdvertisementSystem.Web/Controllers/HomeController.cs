namespace AdvertisementSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
                
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
