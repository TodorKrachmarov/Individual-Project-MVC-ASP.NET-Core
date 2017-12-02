namespace AdvertisementSystem.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;

    [Area("Admin")]
    [Authorize(Roles = AdministratorRole)]
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

        public IActionResult RedirectToAllCategories()
            => this.RedirectToAction(nameof(CategoriesController.All), "Categories");

        public IActionResult RedirectToAllUsers()
            => this.RedirectToAction(nameof(UsersController.All), "Users");
    }
}
