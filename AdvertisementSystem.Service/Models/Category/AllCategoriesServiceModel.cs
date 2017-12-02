namespace AdvertisementSystem.Services.Models.Category
{
    using Common.Mapping;
    using Data.Models;

    public class AllCategoriesServiceModel : IMapFrom<Category>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}
