namespace AdvertisementSystem.Services.Models.Comment
{
    using Common.Mapping;
    using Data.Models;

    public class DeleteCommentServiceModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public int AdId { get; set; }
    }
}
