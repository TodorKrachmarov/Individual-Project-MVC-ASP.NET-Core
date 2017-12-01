namespace AdvertisementSystem.Services.Models.Admin.Users
{
    using Common.Mapping;
    using Data.Models;

    public class UsersListingServiceModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
