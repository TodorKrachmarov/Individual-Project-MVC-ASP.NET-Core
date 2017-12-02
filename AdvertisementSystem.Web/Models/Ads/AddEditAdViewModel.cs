namespace AdvertisementSystem.Web.Models.Ads
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class AddEditAdViewModel
    {
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

        public int Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        [Required]
        public string Keywords { get; set; }
        
    }
}
