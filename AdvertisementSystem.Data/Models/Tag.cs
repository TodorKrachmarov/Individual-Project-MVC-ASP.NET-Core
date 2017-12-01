namespace AdvertisementSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [MinLength(TagNameMinLenght)]
        [MaxLength(TagNameMaxLenght)]
        public string Name { get; set; }

        public List<AdTag> Ads { get; set; } = new List<AdTag>();
    }
}
