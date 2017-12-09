namespace AdvertisementSystem.Web.Models.Manage
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(UserNameMinLenght)]
        [MaxLength(UserNameMaxLenght)]
        public string Name { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
