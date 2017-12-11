namespace AdvertisementSystem.Services.Contracts
{
    public interface ICommentService
    {
        void Create(int adId, string content, string authorId);

        bool AdExist(int id);
    }
}