namespace AdvertisementSystem.Web.Areas.Admin.Models.User
{
    using Services.Models.Admin.Users;
    using System.Collections.Generic;

    public class AdminUsersViewModel
    {
        public IEnumerable<UsersListingServiceModel> Users { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PreviosPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.TotalPages ? this.CurrentPage : this.CurrentPage + 1;
    }
}
