﻿namespace AdvertisementSystem.Services.Implementations
{
    using Data.Models;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models.Admin.Category;
    using System.Collections.Generic;
    using System.Linq;
    using AdvertisementSystem.Services.Models.Admin.Users;

    using static Data.DataConstants;
    using System;

    public class AdminService : IAdminService
    {
        private readonly AdvertisementDbContext db;

        public AdminService(AdvertisementDbContext db)
        {
            this.db = db;
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

        public IEnumerable<UsersListingServiceModel> GetUsers(int page)
            => this.db
                .Users
                .OrderBy(c => c.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ProjectTo<UsersListingServiceModel>()
                .ToList();

        public void DeactivateUser(string id)
        {
            var user = this.db.Users.FirstOrDefault(u => u.Id == id);

            user.IsDeleted = true;

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
    }
}
