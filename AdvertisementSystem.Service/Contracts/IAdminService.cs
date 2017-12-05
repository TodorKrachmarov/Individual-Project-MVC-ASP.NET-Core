namespace AdvertisementSystem.Services.Contracts
{
    using Models.Admin.Category;
    using Models.Admin.Users;
    using System.Collections.Generic;

    public interface IAdminService
    {
        IEnumerable<ListAllCategoriesServiceModel> GetCategories(int page);

        IEnumerable<ListAllCategoriesServiceModel> AllCategories();

        bool CreateCategory(string name, string imageUrl);

        AddEditCategoryServiceModel CategoryToEdit(int id);

        bool EditCategory(int id, string name, string imageUrl);

        bool CategoryNameExists(int id, string name);
        
        bool DeleteCategory(int categorytoDeleteId, int categoryToTransferId);

        string CategoryName(int id);

        IEnumerable<UsersListingServiceModel> GetUsers(int page);

        void DeactivateUser(string id);

        void ActivateUser(string id);

        int AllCategoriesCount();

        int AllUsersCount();

        bool IsDeleted(string id);
    }
}
