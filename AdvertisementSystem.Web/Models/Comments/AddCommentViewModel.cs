namespace AdvertisementSystem.Web.Models.Comments
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class AddCommentViewModel
    {
        [Required]
        [MinLength(CommentContentMinLenght)]
        [MaxLength(CommentContentMaxLenght)]
        public string Content { get; set; }

        public int AdId { get; set; }
    }
}
