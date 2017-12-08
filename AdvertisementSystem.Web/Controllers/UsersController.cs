namespace AdvertisementSystem.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Ads;
    using Services.Contracts;


    public class UsersController : BaseController
    {
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public UsersController(IUserService userService, UserManager<User> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult AdsByUser(string id, int page = 1)
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

            var exist = this.userService.Exist(id);

            if (!exist)
            {
                this.AddErrorMessage("The category you are searching for does not exist!");
                this.RedirectToHome();
            }

            var totalPages = this.userService.TotalAdsByTagCount(id);

            if (page > totalPages)
            {
                page = totalPages;
            }

            if (page < 0)
            {
                page = 1;
            }

            var model = new AdsViewModel
            {
                Ads = this.userService.AdsByUser(id, page),
                Name = this.userService.GetName(id),
                TotalPages = totalPages,
                CurrentPage = page
            };

            return this.View(model);
        }
        
        [AllowAnonymous]
        public IActionResult Profile(string id)
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

            var exist = this.userService.Exist(id);

            if (!exist)
            {
                this.AddErrorMessage("The category you are searching for does not exist!");
                this.RedirectToHome();
            }

            var model = this.userService.GetProfile(id);

            return View(model);
        }
    }
}
