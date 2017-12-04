namespace AdvertisementSystem.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Models.Ads;
    using Services.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static WebConstants;

    [Authorize]
    public class AdsController : BaseController
    {
        private readonly IAdService adService;
        private readonly ICategoryService categoryService;
        private readonly UserManager<User> userManager;

        public AdsController(IAdService adService, ICategoryService categoryService, UserManager<User> userManager)
        {
            this.adService = adService;
            this.categoryService = categoryService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            var model = new AddEditAdViewModel
            {
                Categories = this.Categories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddEditAdViewModel model)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            var categoryExist = this.categoryService.Exist(model.Category);

            if (!ModelState.IsValid || !categoryExist)
            {
                if (!categoryExist)
                {
                    ModelState.AddModelError(nameof(model.Category), "The category does not exist!");
                }

                model.Categories = this.Categories();

                return View(model);
            }

            string authorId = this.userManager.GetUserId(this.User);
            this.adService.Create(model.Title, model.Description, model.ImageUrl, model.Category, model.Keywords, authorId);

            this.AddSuccessMessage($"You created {model.Title} ad!");
            return this.RedirectToHome();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            string authorId = this.userManager.GetUserId(this.User);
            var exist = this.adService.Exists(id, authorId);

            if (!exist || !this.User.IsInRole(AdministratorRole))
            {
                this.AddErrorMessage("The ad you want to edit does not exist!");
                return this.RedirectToHome();
            }

            var ad = this.adService.FindToEdit(id);

            var viewModel = new AddEditAdViewModel
            {
                Title = ad.Title,
                Description = ad.Description,
                ImageUrl = ad.ImageUrl,
                Category = ad.CategoryId,
                Keywords = string.Join(", ", ad.KeyWords),
                Categories = this.Categories()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddEditAdViewModel model)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            string authorId = this.userManager.GetUserId(this.User);
            var exist = this.adService.Exists(id, authorId);

            if (!exist || !this.User.IsInRole(AdministratorRole))
            {
                this.AddErrorMessage("The ad you want to edit does not exist!");
                return this.RedirectToHome();
            }

            var categoryExist = this.categoryService.Exist(model.Category);

            if (!ModelState.IsValid || !categoryExist)
            {
                if (!categoryExist)
                {
                    ModelState.AddModelError(nameof(model.Category), "The category does not exist!");
                }

                model.Categories = this.Categories();

                return View(model);
            }

            this.adService.Edit(id, model.Title, model.Description, model.ImageUrl, model.Category, model.Keywords);

            this.AddSuccessMessage($"You edited {model.Title} ad!");
            return this.RedirectToHome();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            string authorId = this.userManager.GetUserId(this.User);
            var exist = this.adService.Exists(id, authorId);

            if (!exist || !this.User.IsInRole(AdministratorRole))
            {
                this.AddErrorMessage("The ad you want to edit does not exist!");
                return this.RedirectToHome();
            }

            return this.View(id);
        }

        public async Task<IActionResult> Destroy(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            string authorId = this.userManager.GetUserId(this.User);
            var exist = this.adService.Exists(id, authorId);

            if (!exist || !this.User.IsInRole(AdministratorRole))
            {
                this.AddErrorMessage("The ad you want to edit does not exist!");
                return this.RedirectToHome();
            }

            var name = this.adService.Delete(id);

            this.AddSuccessMessage($"You deleted {name} ad!");
            return this.RedirectToHome();
        }
        
        private IEnumerable<SelectListItem> Categories()
            => this.categoryService
                                  .All()
                                  .Select(c => new SelectListItem
                                  {
                                      Value = c.Id.ToString(),
                                      Text = c.Name
                                  })
                                  .ToList();
    }
}
