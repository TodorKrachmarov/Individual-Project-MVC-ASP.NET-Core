namespace AdvertisementSystem.Services.Models.Comment
{
    using Common.Mapping;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;

    using static Data.DataConstants;

    public class EditCommentServiceModel : IMapFrom<Comment>
    {
        [Required]
        [MinLength(CommentContentMinLenght)]
        [MaxLength(CommentContentMaxLenght)]
        public string Content { get; set; }
    }
}
