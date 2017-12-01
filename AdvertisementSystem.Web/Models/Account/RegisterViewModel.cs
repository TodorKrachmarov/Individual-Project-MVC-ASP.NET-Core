namespace AdvertisementSystem.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [MinLength(UserNameMinLenght)]
        [MaxLength(UserNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^\+[0-9]{10,12}$", ErrorMessage = "Phone must start with “+” sign followed by 10 to 12 digits.")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
