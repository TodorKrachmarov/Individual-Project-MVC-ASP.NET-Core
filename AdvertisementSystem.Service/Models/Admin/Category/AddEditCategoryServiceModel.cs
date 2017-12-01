namespace AdvertisementSystem.Services.Models.Admin.Category
{
    using Common.Mapping;
    using Data.Models;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class AddEditCategoryServiceModel : IMapFrom<Category>
    {
        [Required]
        [MinLength(CategoryNameMinLenght)]
        [MaxLength(CategoryNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLenght)]
        public string ImageUrl { get; set; }
    }
}
