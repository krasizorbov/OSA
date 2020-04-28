namespace OSA.Web.ViewModels.Purchases.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class ShowPurchaseByCompanyInputModel
    {
        [BindRequired]
        [Display(Name = GlobalConstants.DisplayCompany)]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }

        [Display(Name = GlobalConstants.DisplayStartDate)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string StartDate { get; set; }

        [Display(Name = GlobalConstants.DisplayEndDate)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string EndDate { get; set; }
    }
}
