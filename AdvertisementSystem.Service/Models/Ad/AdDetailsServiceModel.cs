namespace AdvertisementSystem.Services.Models.Ad
{
    using Tag;
    using System;
    using System.Collections.Generic;
    using Common.Mapping;
    using Data.Models;
    using AutoMapper;
    using System.Linq;
    using Comment;

    public class AdDetailsServiceModel : IMapFrom<Ad>, IHaveCustomMapping
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string ImageUrl { get; set; }

        public DateTime PublishDate { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }
        
        public string AuthorEmail { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
        
        public IEnumerable<TagServiceModel> Tags { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                    .CreateMap<Ad, AdDetailsServiceModel>()
                    .ForMember(e => e.Tags, cfg => cfg.MapFrom(a => a.Tags.Select(t => t.Tag)))
                    .ForMember(e => e.AuthorName, cfg => cfg.MapFrom(a => a.Author.Name))
                    .ForMember(e => e.AuthorEmail, cfg => cfg.MapFrom(a => a.Author.Email))
                    .ForMember(e => e.CategoryName, cfg => cfg.MapFrom(a => a.Category.Name));
    }
}
