namespace AdvertisementSystem.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;

    public class CategoriesController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public CategoriesController(ICategoryService categoryService, IUserService userService, UserManager<User> userManager)
        {
            this.categoryService = categoryService;
            this.userService = userService;
            this.userManager = userManager;
        }

        public IActionResult AdsByCategory(int id)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            var exist = this.categoryService.Exist(id);

            if (!exist)
            {
                this.AddErrorMessage("The category you are searching for does not exist!");
                this.RedirectToHome();
            }

            var model = this.categoryService.AdsByCategory(id);

            return this.View(model);
        }
    }
}
