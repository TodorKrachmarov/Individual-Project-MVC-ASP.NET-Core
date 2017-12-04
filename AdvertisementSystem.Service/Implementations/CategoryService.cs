namespace AdvertisementSystem.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models.Category;
    using System.Collections.Generic;
    using System.Linq;

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
    }
}
