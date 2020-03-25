namespace OSA.Web.ViewModels.Purchases.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreatePurchaseInputModel
    {
        [BindRequired]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }

        [Required]
        [Display(Name = "Start date of the month - (dd/mm/yyyy)")]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "End date of the month - (dd/mm/yyyy)")]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string EndDate { get; set; }
    }
}
