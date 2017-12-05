namespace AdvertisementSystem.Services.Models.Category
{
    using Common.Mapping;
    using Data.Models;

    public class CategoriesHomeServiceModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}
