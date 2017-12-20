namespace AdvertisementSystem.Services.Implementations
{
    using AdvertisementSystem.Services.Models.Admin.Users;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Models.Admin.Category;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Data.DataConstants;

    public class AdminService : IAdminService
    {
        private readonly AdvertisementDbContext db;
        private readonly UserManager<User> userManager;

        public AdminService(AdvertisementDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public IEnumerable<ListAllCategoriesServiceModel> GetCategories(int page)
            => this.db
                    .Categories
                    .OrderBy(c => c.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ProjectTo<ListAllCategoriesServiceModel>()
                    .ToList();

        public IEnumerable<ListAllCategoriesServiceModel> AllCategories()
            => this.db
                    .Categories
                    .ProjectTo<ListAllCategoriesServiceModel>()
                    .ToList();

        public bool CreateCategory(string name, string imageUrl)
        {
            if (this.db.Categories.Any(c => c.Name == name))
            {
                return false;
            }

            this.db.Categories.Add(new Category
            {
                Name = name,
                ImageUrl = imageUrl
            });

            this.db.SaveChanges();

            return true;
        }

        public AddEditCategoryServiceModel CategoryToEdit(int id)
            => this.db
                    .Categories
                    .Where(c => c.Id == id)
                    .ProjectTo<AddEditCategoryServiceModel>()
                    .FirstOrDefault();

        public bool CategoryNameExists(int id, string name)
        {
            var category = this.db.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return false;
            }

            if (category.Name != name)
            {
                if (this.db.Categories.Any(c => c.Name == name))
                {
                    return true;
                }
            }

            return false;
        }

        public bool EditCategory(int id, string name, string imageUrl)
        {
            var category = this.db.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return false;
            }

            category.Name = name;
            category.ImageUrl = imageUrl;

            this.db.SaveChanges();

            return true;
        }

        public bool DeleteCategory(int categorytoDeleteId, int categoryToTransferId)
        {
            var categoryToDelete = this.db.Categories.FirstOrDefault(c => c.Id == categorytoDeleteId);
            var categoryToTransfer = this.db.Categories.FirstOrDefault(c => c.Id == categoryToTransferId);

            if (categoryToDelete == null
                || categoryToTransfer == null)
            {
                return false;
            }

            var ads = this.db.Ads.Where(a => a.CategoryId == categorytoDeleteId);

            foreach (var ad in ads)
            {
                ad.CategoryId = categoryToTransferId;
            }

            this.db.SaveChanges();

            this.db.Categories.Remove(categoryToDelete);
            this.db.SaveChanges();

            return true;
        }

        public string CategoryName(int id)
            => this.db.Categories.FirstOrDefault(c => c.Id == id).Name;

        public async Task<IEnumerable<UsersListingServiceModel>> GetUsers(int page, string searchTerm)
        {
            searchTerm = searchTerm ?? string.Empty;

            var users = this.db
                            .Users
                            .Where(u => u.Email.ToLower().Contains(searchTerm.ToLower()))
                            .OrderBy(c => c.Name)
                            .Skip((page - 1) * PageSize)
                            .Take(PageSize)
                            .ProjectTo<UsersListingServiceModel>()
                            .ToList();

            foreach (var user in users)
            {
                var us = await this.userManager.FindByIdAsync(user.Id);
                if (await this.userManager.IsInRoleAsync(us, AdministratorRole))
                {
                    user.IsAdmin = true;
                }
            }

            return users;
        }
            
        public void DeactivateUser(string id)
        {
            var user = this.db.Users.FirstOrDefault(u => u.Id == id);

            user.IsDeleted = true;

            var ads = this.db.Ads.Where(a => a.AuthorId == id);

            foreach (var ad in ads)
            {
                this.db.Ads.Remove(ad);
            }

            var comments = this.db.Comments.Where(c => c.AuthorId == id);

            foreach (var comment in comments)
            {
                this.db.Comments.Remove(comment);
            }

            this.db.SaveChanges();
        }

        public void ActivateUser(string id)
        {
            var user = this.db.Users.FirstOrDefault(u => u.Id == id);

            user.IsDeleted = false;

            this.db.SaveChanges();
        }

        public int AllCategoriesCount()
            => (int)Math.Ceiling(this.db.Categories.Count() / (double)PageSize);

        public int AllUsersCount()
            => (int)Math.Ceiling(this.db.Users.Count() / (double)PageSize);

        public bool IsDeleted(string id)
            => this.db
                    .Users
                    .Where(u => u.Id == id)
                    .Select(u => u.IsDeleted)
                    .FirstOrDefault();
    }
}
