namespace AdvertisementSystem.Services.Models.Users
{
    using Data.Models;
    using Common.Mapping;
    using AutoMapper;

    public class UserProfileServiceModel : IMapFrom<User>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int AdsCount { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                    .CreateMap<User, UserProfileServiceModel>()
                    .ForMember(e => e.AdsCount, cfg => cfg.MapFrom(u => u.Ads.Count));
    }
}
