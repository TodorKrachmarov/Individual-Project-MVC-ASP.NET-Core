namespace AdvertisementSystem.Services.Contracts
{
    using Models.Ad;
    using System.Collections.Generic;

    public interface IAdService
    {
        void Create(
            string title,
            string description,
            string imageUrl,
            int CategoryId,
            string keyWords,
            string authorId);

        bool Exists(int adId, string authorId);

        EditAdServiceModel FindToEdit(int id);

        bool Edit(
            int id,
            string title,
            string description,
            string imageUrl,
            int CategoryId,
            string keyWords);

        string Delete(int id);

        bool ReadyToDelete(int id);
    }
}
