namespace AdvertisementSystem.Services.Models.Ad
{
    using Common.Mapping;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using Tag;
    using AutoMapper;
    using System.Linq;

    public class ListAdsServiceModel : IMapFrom<Ad>, IHaveCustomMapping
    {
        public int Id { get; set; }
        
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublishDate { get; set; }

        public IEnumerable<TagServiceModel> Tags { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                    .CreateMap<Ad, ListAdsServiceModel>()
                    .ForMember(e => e.Tags, cfg => cfg.MapFrom(a => a.Tags.Select(t => t.Tag)));
    }
}
