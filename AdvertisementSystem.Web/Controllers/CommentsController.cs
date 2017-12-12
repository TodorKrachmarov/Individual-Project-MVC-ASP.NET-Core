namespace AdvertisementSystem.Web.Controllers
{
    using AdvertisementSystem.Services.Models.Comment;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Comments;
    using Services.Contracts;

    using static WebConstants;

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

        public IActionResult Edit(int id)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            if (!this.commentService.CommentExist(id))
            {
                this.AddErrorMessage("The comment you are trying to edit does not exist!");
                return this.RedirectToHome();
            }

            var exist = this.commentService.Exist(id, userId);

            if (!exist && !this.User.IsInRole(AdministratorRole))
            {
                this.AddErrorMessage("The comment you are trying to edit does not exist!");
                return this.RedirectToHome();
            }

            var model = this.commentService.FindToEdit(id);

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, EditCommentServiceModel model)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            if (!this.commentService.CommentExist(id))
            {
                this.AddErrorMessage("The comment you are trying to edit does not exist!");
                return this.RedirectToHome();
            }

            var exist = this.commentService.Exist(id, userId);

            if (!exist && !this.User.IsInRole(AdministratorRole))
            {
                this.AddErrorMessage("The comment you are trying to edit does not exist!");
                return this.RedirectToHome();
            }

            var adId = this.commentService.Edit(id, model.Content);

            this.AddSuccessMessage($"You edited the comment!");
            return this.RedirectToAction(nameof(AdsController.Details), "Ads", new { id = adId});
        }

        public IActionResult Delete(int id)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            if (!this.commentService.CommentExist(id))
            {
                this.AddErrorMessage("The comment you are trying to delete does not exist!");
                return this.RedirectToHome();
            }

            var exist = this.commentService.Exist(id, userId);

            if (!exist && !this.User.IsInRole(AdministratorRole))
            {
                this.AddErrorMessage("The comment you are trying to delete does not exist!");
                return this.RedirectToHome();
            }

            var model = this.commentService.FindToDelete(id);

            return this.View(model);
        }

        public IActionResult Destroy(int id)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            if (!this.commentService.CommentExist(id))
            {
                this.AddErrorMessage("The comment you are trying to delete does not exist!");
                return this.RedirectToHome();
            }

            var exist = this.commentService.Exist(id, userId);

            if (!exist && !this.User.IsInRole(AdministratorRole))
            {
                this.AddErrorMessage("The comment you are trying to delete does not exist!");
                return this.RedirectToHome();
            }

            var adId = this.commentService.Delete(id);

            this.AddSuccessMessage($"You deleted the comment!");
            return this.RedirectToAction(nameof(AdsController.Details), "Ads", new { id = adId });
        }
    }
}
