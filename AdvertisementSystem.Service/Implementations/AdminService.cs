namespace AdvertisementSystem.Services.Implementations
{
    using AdvertisementSystem.Data.Models;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models.Admin.Category;
    using System.Collections.Generic;
    using System.Linq;
    using AdvertisementSystem.Services.Models.Admin.Users;

    public class AdminService : IAdminService
    {
        private readonly AdvertisementDbContext db;

        public AdminService(AdvertisementDbContext db)
        {
            this.db = db;
        }

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

            if (category.Name != name)
            {
                if (this.db.Categories.Any(c => c.Name == name))
                {
                    return false;
                }
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

        public IEnumerable<UsersListingServiceModel> GetAllUsers()
            => this.db
                .Users
            .OrderBy(c => c.Name)
            .ProjectTo<UsersListingServiceModel>()
            .ToList();
    }
}
