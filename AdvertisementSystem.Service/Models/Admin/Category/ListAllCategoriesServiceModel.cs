namespace AdvertisementSystem.Services.Models.Admin.Category
{
    using Common.Mapping;
    using Data.Models;

    public class ListAllCategoriesServiceModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
