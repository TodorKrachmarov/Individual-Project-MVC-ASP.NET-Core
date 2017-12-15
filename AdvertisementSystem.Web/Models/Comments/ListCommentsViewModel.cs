namespace AdvertisementSystem.Web.Models.Comments
{
    using Services.Models.Comment;
    using System.Collections.Generic;

    public class ListCommentsViewModel
    {
        public IEnumerable<ListCommentServiceModel> Comments { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PreviosPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage >= this.TotalPages ? this.CurrentPage : this.CurrentPage + 1;
    }
}
