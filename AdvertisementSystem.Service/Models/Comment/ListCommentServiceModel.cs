namespace AdvertisementSystem.Services.Models.Comment
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;

    public class ListCommentServiceModel : IMapFrom<Comment>, IHaveCustomMapping
    {

        public int Id { get; set; }

        public string Content { get; set; }
        
        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                    .CreateMap<Comment, ListCommentServiceModel>()
                    .ForMember(e => e.AuthorName, cfg => cfg.MapFrom(c => c.Author.Name))
                    .ForMember(e => e.AuthorEmail, cfg => cfg.MapFrom(c => c.Author.Email));
    }
}
