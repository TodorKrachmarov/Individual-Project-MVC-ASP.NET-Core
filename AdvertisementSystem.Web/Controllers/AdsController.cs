namespace AdvertisementSystem.Web.Controllers
{
    using Services.Contracts;
    using Models.Ads;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Identity;
    using Data.Models;
    using System.Collections.Generic;

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

        public IActionResult Create()
        {
            var model = new AddEditAdViewModel
            {
                Categories = this.Categories()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(AddEditAdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = this.Categories();

                return View(model);
            }

            string authorId = this.userManager.GetUserId(this.User);
            this.adService.Create(model.Title, model.Description, model.ImageUrl, model.Category, model.Keywords, authorId);

            this.AddSuccessMessage($"You created {model.Title} ad!");
            return this.RedirectToHome();
        }

        public IActionResult Edit(int id)
        {
            string authorId = this.userManager.GetUserId(this.User);
            var exist = this.adService.Exists(id, authorId);

            if (!exist)
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
        public IActionResult Edit(int id, AddEditAdViewModel model)
        {
            string authorId = this.userManager.GetUserId(this.User);
            var exist = this.adService.Exists(id, authorId);

            if (!exist)
            {
                this.AddErrorMessage("The ad you want to edit does not exist!");
                return this.RedirectToHome();
            }

            if (!ModelState.IsValid)
            {
                model.Categories = this.Categories();

                return View(model);
            }

            this.adService.Edit(id, model.Title, model.Description, model.ImageUrl, model.Category, model.Keywords);

            this.AddSuccessMessage($"You edited {model.Title} ad!");
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
