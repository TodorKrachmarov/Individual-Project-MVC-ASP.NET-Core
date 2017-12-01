namespace AdvertisementSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Ad
    {
        public int Id { get; set; }

        [Required]
        [MinLength(AdTitleMinLenght)]
        [MaxLength(AdTitleMaxLenght)]
        public string Title { get; set; }

        [Required]
        [MinLength(AdDescriptionMinLenght)]
        [MaxLength(AdDescriptionMaxLenght)]
        public string Description { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLenght)]
        public string ImageUrl { get; set; }

        public DateTime PublishDate { get; set; }

        public string AuthorId { get; set; }

        public User Author { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<AdTag> Tags { get; set; } = new List<AdTag>();
    }
}
