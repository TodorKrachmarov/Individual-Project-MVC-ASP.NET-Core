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

    using static WebConstants;
    
    public class AdsController : BaseController
    {
        private readonly IAdService adService;
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public AdsController(IAdService adService, ICategoryService categoryService, UserManager<User> userManager, IUserService userService)
        {
            this.adService = adService;
            this.categoryService = categoryService;
            this.userManager = userManager;
            this.userService = userService;
        }

        public IActionResult Create()
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
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
        public IActionResult Create(AddEditAdViewModel model)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
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
            
            this.adService.Create(model.Title, model.Description, model.ImageUrl, model.Category, model.Keywords, userId);

            this.AddSuccessMessage($"You created {model.Title} ad!");
            return this.RedirectToAction(nameof(CategoriesController.AdsByCategory), "Categories", new { id = model.Category});
        }

        public IActionResult Edit(int id)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }
            
            var exist = this.adService.Exists(id, userId);

            if (!exist && !this.User.IsInRole(AdministratorRole))
            {
                this.AddErrorMessage("The ad you want to edit does not exist!");
                return this.RedirectToHome();
            }

            var ad = this.adService.FindToEdit(id);

            if (ad == null)
            {
                this.AddErrorMessage("The ad you want to edit does not exist!");
                return this.RedirectToHome();
            }

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
        public IActionResult Edit(int id, AddEditAdViewModel model)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }
            
            var exist = this.adService.Exists(id, userId);

            if (!exist && !this.User.IsInRole(AdministratorRole))
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

            var success = this.adService.Edit(id, model.Title, model.Description, model.ImageUrl, model.Category, model.Keywords);

            if (!success)
            {
                this.AddErrorMessage("The ad you want to edit does not exist!");
                return this.RedirectToHome();
            }

            this.AddSuccessMessage($"You edited {model.Title} ad!");
            return this.RedirectToHome();
        }

        public IActionResult Delete(int id)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            var exist = this.adService.Exists(id, userId);

            if (!exist && !this.User.IsInRole(AdministratorRole))
            {
                this.AddErrorMessage("The ad you want to delete does not exist!");
                return this.RedirectToHome();
            }

            if (!this.adService.ReadyToDelete(id))
            {
                this.AddErrorMessage("The ad you want to delete does not exist!");
                return this.RedirectToHome();
            }

            return this.View(id);
        }

        public IActionResult Destroy(int id)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.userService.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            var exist = this.adService.Exists(id, userId);

            if (!exist && !this.User.IsInRole(AdministratorRole))
            {
                this.AddErrorMessage("The ad you want to delete does not exist!");
                return this.RedirectToHome();
            }

            var name = this.adService.Delete(id);

            if (name == null)
            {
                this.AddErrorMessage("The ad you want to delete does not exist!");
                return this.RedirectToHome();
            }

            this.AddSuccessMessage($"You deleted {name} ad!");
            return this.RedirectToHome();
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
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

            var model = this.adService.Details(id);

            if (model == null)
            {
                this.AddErrorMessage("The ad you are searching for does not exist!");
                return this.RedirectToHome();
            }

            return this.View(model);
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
