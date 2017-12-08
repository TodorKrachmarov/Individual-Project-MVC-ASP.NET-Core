namespace AdvertisementSystem.Services.Contracts
{
    using Models.Ad;
    using System.Collections.Generic;

    public interface ITagService
    {
        bool Exist(int id);
        
        IEnumerable<ListAdsServiceModel> AdsByTag(int id, int page);

        int TotalAdsByTagCount(int id);

        string GetName(int id);
    }
}
