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

        public int VerifyPageValue(int page, int totalPages)
        {
            if (page > totalPages)
            {
                if (totalPages > 0)
                {
                    return totalPages;
                }

                return 1;
            }

            if (page <= 0)
            {
                return 1;
            }

            return page;
        }
    }
}
