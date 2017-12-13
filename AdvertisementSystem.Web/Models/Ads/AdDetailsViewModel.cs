namespace AdvertisementSystem.Web.Models.Ads
{
    using Comments;
    using Services.Models.Tag;
    using System;
    using System.Collections.Generic;

    public class AdDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublishDate { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
                
        public IEnumerable<TagServiceModel> Tags { get; set; }

        public ListCommentsViewModel Comments { get; set; }
    }
}
