namespace AdvertisementSystem.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Ads;
    using Services.Contracts;


    public class TagsController : BaseController
    {
        private readonly ITagService tagService;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public TagsController(ITagService tagService, IUserService userService, UserManager<User> userManager)
        {
            this.tagService = tagService;
            this.userService = userService;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult AdsByTag(int id, int page = 1)
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

            var exist = this.tagService.Exist(id);

            if (!exist)
            {
                this.AddErrorMessage("The category you are searching for does not exist!");
                this.RedirectToHome();
            }

            var totalPages = this.tagService.TotalAdsByTagCount(id);

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
                Ads = this.tagService.AdsByTag(id, page),
                Name = this.tagService.GetName(id),
                TotalPages = totalPages,
                CurrentPage = page
            };

            return this.View(model);
        }
    }
}
