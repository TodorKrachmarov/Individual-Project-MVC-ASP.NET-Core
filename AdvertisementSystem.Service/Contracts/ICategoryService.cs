namespace AdvertisementSystem.Services.Contracts
{
    using Models.Ad;
    using Models.Category;
    using System.Collections.Generic;

    public interface ICategoryService
    {
        IEnumerable<AllCategoriesServiceModel> All();

        bool Exist(int id);

        IEnumerable<CategoriesHomeServiceModel> CategoriesToView();

        IEnumerable<ListAdsServiceModel> AdsByCategory(int id, int page);

        int TotalAdsByCategoryCount(int id);

        string GetName(int id);
    }
}
