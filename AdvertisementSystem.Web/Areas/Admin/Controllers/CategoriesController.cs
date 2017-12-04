﻿namespace AdvertisementSystem.Web.Areas.Admin.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Models.Category;
    using Services.Contracts;
    using Services.Models.Admin.Category;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoriesController : BaseController
    {
        private readonly IAdminService admin;
        private readonly UserManager<User> userManager;

        public CategoriesController(IAdminService admin, UserManager<User> userManager)
        {
            this.admin = admin;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(int page = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            var model = new AdminCategoryViewModel
            {
                Categories = this.admin.GetCategories(page),
                CurrentPage = page,
                TotalPages = this.admin.AllCategoriesCount()
            };

            return this.View(model);
        }

        public async Task<IActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddEditCategoryServiceModel model)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var success = this.admin.CreateCategory(model.Name, model.ImageUrl);

            if (!success)
            {
                this.ModelState.AddModelError(nameof(model.Name), "Category with this name already exists!");
                return this.View(model);
            }

            this.AddSuccessMessage($"You successfuly created {model.Name} category.");

            return this.RedirectToAllCategories();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            var category = this.admin.CategoryToEdit(id);

            if (category == null)
            {
                this.AddErrorMessage("The category you are trying to edit does not exist!");
                return RedirectToAllCategories();
            }

            return this.View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddEditCategoryServiceModel model)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var nameExists = this.admin.CategoryNameExists(id, model.Name);

            if (nameExists)
            {
                this.ModelState.AddModelError(nameof(model.Name), "Category with this name already exists!");
                return this.View(model);
            }

            var success = this.admin.EditCategory(id, model.Name, model.ImageUrl);

            if (!success)
            {
                this.AddErrorMessage("The category you are trying to edit does not exist!");
                return RedirectToAllCategories();
            }

            this.AddSuccessMessage($"You successfuly edited {model.Name} category.");

            return this.RedirectToAllCategories();
        }

        public async Task<IActionResult> Delete()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            var model = new DeleteCategoryViewModel
            {
                Categories = this.CategoriesToList()
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCategoryViewModel model)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.IsDeleted)
            {
                return BadRequest();
            }

            if (model.CategoryToDelete == model.CategoryToTransfer)
            {
                this.ModelState.AddModelError("", "Category to delete must be different from category to transfer!");
                model.Categories = this.CategoriesToList();
                return this.View(model);
            }

            var name = this.admin.CategoryName(model.CategoryToDelete);
            var success = this.admin.DeleteCategory(model.CategoryToDelete, model.CategoryToTransfer);

            if (!success)
            {
                this.AddErrorMessage("Either the category you are trying to delete does not exist or the category you want to transfer the ads does not exist!");
                return RedirectToAllCategories();
            }
            
            this.AddSuccessMessage($"You successfuly deleted {name} category.");

            return this.RedirectToAllCategories();
        }

        private IEnumerable<SelectListItem> CategoriesToList()
                => this.admin.AllCategories()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
    }
}
