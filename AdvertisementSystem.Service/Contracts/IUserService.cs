namespace AdvertisementSystem.Services.Contracts
{
    using Models.Users;
    using Models.Ad;
    using System.Collections.Generic;

    public interface IUserService
    {
        bool IsDeleted(string id);

        bool Exist(string id);

        IEnumerable<ListAdsServiceModel> AdsByUser(string id, int page);

        int TotalAdsByTagCount(string id);

        string GetName(string id);

        UserProfileServiceModel GetProfile(string id);
    }
}
