namespace AdvertisementSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [MinLength(CommentContentMinLenght)]
        [MaxLength(CommentContentMaxLenght)]
        public string Content { get; set; }

        public string AuthorId { get; set; }

        public User Author { get; set; }

        public int AdId { get; set; }

        public Ad Ad { get; set; }
    }
}
