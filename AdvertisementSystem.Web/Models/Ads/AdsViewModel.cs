namespace AdvertisementSystem.Web.Models.Ads
{
    using Services.Models.Ad;
    using System.Collections.Generic;

    public class AdsViewModel
    {
        public IEnumerable<ListAdsServiceModel> Ads { get; set; }

        public string Name { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PreviosPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage >= this.TotalPages ? this.CurrentPage : this.CurrentPage + 1;
    }
}
