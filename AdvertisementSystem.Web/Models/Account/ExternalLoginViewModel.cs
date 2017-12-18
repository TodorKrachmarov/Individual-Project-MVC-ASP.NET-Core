namespace AdvertisementSystem.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(UserNameMinLenght)]
        [MaxLength(UserNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^\+[0-9]{10,12}$", ErrorMessage = "Phone must start with “+” sign followed by 10 to 12 digits.")]
        public string Phone { get; set; }
    }
}
