namespace AdvertisementSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BaseController : Controller
    {
        public void AddSuccessMessage(string message)
        {
            this.TempData["Success"] = message;
        }

        public void AddErrorMessage(string message)
        {
            this.TempData["Error"] = message;
        }

        public IActionResult RedirectToHome()
            => this.RedirectToAction(nameof(HomeController.Index), "Home");
    }
}
