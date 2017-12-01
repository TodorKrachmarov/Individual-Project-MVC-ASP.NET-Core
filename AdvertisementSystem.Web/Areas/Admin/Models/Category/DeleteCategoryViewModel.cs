namespace AdvertisementSystem.Web.Areas.Admin.Models.Category
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DeleteCategoryViewModel
    {
        [Required]
        [Display(Name = "Category To Delete:")]
        public int CategoryToDelete { get; set; }

        [Required]
        [Display(Name = "Category To Transfer:")]
        public int CategoryToTransfer { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
