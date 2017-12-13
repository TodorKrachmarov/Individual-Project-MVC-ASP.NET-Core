namespace AdvertisementSystem.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Ads;
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

        [AllowAnonymous]
        public IActionResult AdsByCategory(int id, int page = 1)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                string userId = this.userManager.GetUserId(this.User);
                var isDeleted = this.userService.IsDeleted(userId);

                if (isDeleted)
                {
                    return BadRequest();
                }
            }

            var exist = this.categoryService.Exist(id);

            if (!exist)
            {
                this.AddErrorMessage("The category you are searching for does not exist!");
                this.RedirectToHome();
            }

            var totalPages = this.categoryService.TotalAdsByCategoryCount(id);
            page = this.VerifyPageValue(page, totalPages);

            var model = new AdsViewModel
            {
                Ads = this.categoryService.AdsByCategory(id, page),
                Name = this.categoryService.GetName(id),
                TotalPages = totalPages,
                CurrentPage = page
            };

            return this.View(model);
        }
    }
}
