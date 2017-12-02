namespace AdvertisementSystem.Services.Models.Ad
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using static Data.DataConstants;
    
    public class EditAdServiceModel : IMapFrom<Ad>, IHaveCustomMapping
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

        public int CategoryId { get; set; }

        [Required]
        public IEnumerable<string> KeyWords { get; set; } = new List<string>();

        public void ConfigureMapping(Profile mapper)
            => mapper
                    .CreateMap<Ad, EditAdServiceModel>()
                    .ForMember(e => e.KeyWords, cfg => cfg.MapFrom(a => a.Tags.Select(t => t.Tag.Name).ToList()));
    }
}
