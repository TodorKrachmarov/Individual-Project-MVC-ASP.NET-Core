namespace AdvertisementSystem.Web.Areas.Admin.Models.Category
{
    using Services.Models.Admin.Category;
    using System.Collections.Generic;

    public class AdminCategoryViewModel
    {
        public IEnumerable<ListAllCategoriesServiceModel> Categories { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PreviosPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.TotalPages ? this.CurrentPage : this.CurrentPage + 1; 
    }
}
