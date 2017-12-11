namespace AdvertisementSystem.Services.Implementations
{
    using Contracts;
    using Data;
    using Data.Models;
    using System.Linq;

    public class CommentService : ICommentService
    {
        private readonly AdvertisementDbContext db;

        public CommentService(AdvertisementDbContext db)
        {
            this.db = db;
        }

        public void Create(int adId, string content, string authorId)
        {
            var comment = new Comment
            {
                Content = content,
                AdId = adId,
                AuthorId = authorId
            };

            this.db.Comments.Add(comment);
            this.db.SaveChanges();
        }

        public bool AdExist(int id)
            => this.db.Ads.Any(a => a.Id == id);
    }
}
