namespace AdvertisementSystem.Services.Implementations
{
    using Models.Ad;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models.Category;
    using System.Collections.Generic;
    using System.Linq;

    using static Data.DataConstants;
    using System;

    public class CategoryService : ICategoryService
    {
        private readonly AdvertisementDbContext db;

        public CategoryService(AdvertisementDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<AllCategoriesServiceModel> All()
            => this.db
                    .Categories
                    .ProjectTo<AllCategoriesServiceModel>()
                    .ToList();

        public bool Exist(int id)
            => this.db.Categories.Any(c => c.Id == id);

        public IEnumerable<CategoriesHomeServiceModel> CategoriesToView()
            => this.db
                    .Categories
                    .ProjectTo<CategoriesHomeServiceModel>()
                    .ToList();

        public IEnumerable<ListAdsServiceModel> AdsByCategory(int id, int page)
            => this.db
                    .Ads
                    .Where(a => a.CategoryId == id)
                    .OrderByDescending(a => a.PublishDate)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ProjectTo<ListAdsServiceModel>()
                    .ToList();

        public string GetName(int id)
            => this.db.Categories.FirstOrDefault(c => c.Id == id).Name;

        public int TotalAdsByCategoryCount(int id)
        { 
            var count = this.db
                            .Ads
                            .Where(a => a.CategoryId == id)
                            .Count();

            var num = Math.Ceiling(count / (double)PageSize);

            return (int)num;
        }
    }
}
