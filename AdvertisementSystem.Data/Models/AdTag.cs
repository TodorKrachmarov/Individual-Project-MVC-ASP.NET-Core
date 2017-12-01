namespace AdvertisementSystem.Data.Models
{
    public class AdTag
    {
        public int AdId { get; set; }

        public Ad Ad { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
