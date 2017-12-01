namespace AdvertisementSystem.Web.Areas.Admin.Models.User
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeUserPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password:")]
        public string NewPassword { get; set; }
    }
}
