namespace AdvertisementSystem.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class User : IdentityUser
    {
        [Required]
        [MinLength(UserNameMinLenght)]
        [MaxLength(UserNameMaxLenght)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<Ad> Ads { get; set; } = new List<Ad>();
    }
}
