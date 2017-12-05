namespace AdvertisementSystem.Services.Models.Tag
{
    using Common.Mapping;
    using Data.Models;

    public class TagServiceModel : IMapFrom<Tag>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
