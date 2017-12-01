namespace AdvertisementSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MinLength(CategoryNameMinLenght)]
        [MaxLength(CategoryNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLenght)]
        public string ImageUrl { get; set; }

        public List<Ad> Ads { get; set; } = new List<Ad>();
    }
}
