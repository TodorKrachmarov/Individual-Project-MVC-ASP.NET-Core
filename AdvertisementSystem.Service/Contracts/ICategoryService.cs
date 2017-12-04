namespace AdvertisementSystem.Services.Contracts
{
    using Models.Category;
    using System.Collections.Generic;

    public interface ICategoryService
    {
        IEnumerable<AllCategoriesServiceModel> All();

        bool Exist(int id);
    }
}
