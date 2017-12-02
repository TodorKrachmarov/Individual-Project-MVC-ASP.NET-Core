namespace AdvertisementSystem.Services.Contracts
{
    using Models.Ad;

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

        void Edit(
            int id,
            string title,
            string description,
            string imageUrl,
            int CategoryId,
            string keyWords);
    }
}
