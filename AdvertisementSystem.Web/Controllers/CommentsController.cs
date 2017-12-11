namespace AdvertisementSystem.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Comments;
    using Services.Contracts;

    public class CommentsController : BaseController
    {
        private readonly ICommentService commentService;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public CommentsController(ICommentService commentService, IUserService userService, UserManager<User> userManager)
        {
            this.commentService = commentService;
            this.userService = userService;
            this.userManager = userManager;
        }

        [HttpPost]
        public IActionResult Create(AddCommentViewModel model)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            if (!this.commentService.AdExist(model.AdId))
            {
                this.AddErrorMessage("The ad you are trying to comment does not exist!");
                return this.RedirectToHome();
            }

            if (!ModelState.IsValid)
            {
                this.TempData["commentError"] = "The comment is required and must be at least 20 symbols but not more than 200 symbols!";
                return RedirectToAction(nameof(AdsController.Details), "Ads", new { id = model.AdId });
            }

            this.commentService.Create(model.AdId, model.Content, userId);

            return RedirectToAction(nameof(AdsController.Details), "Ads", new { id = model.AdId });
        }
    }
}
