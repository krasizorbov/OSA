namespace OSA.Web.ViewModels.Suppliers.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreateSupplierInputModel
    {
        private const string BulstatFormat = "[0-9]*";

        [Required]
        [MaxLength(GlobalConstants.CompanyNameMaxLength)]
        [Display(Name = "Supplier")]
        public string Name { get; set; }

        [Required]
        [MinLength(GlobalConstants.BulstatMinLength)]
        [MaxLength(GlobalConstants.BulstatMaxLength)]
        [RegularExpression(BulstatFormat, ErrorMessage = GlobalConstants.InvalidBulstat)]
        [Display(Name = "Bulstat")]
        public string Bulstat { get; set; }

        [BindRequired]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }
    }
}
