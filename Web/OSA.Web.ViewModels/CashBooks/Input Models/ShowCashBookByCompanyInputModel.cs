namespace OSA.Web.ViewModels.CashBooks.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class ShowCashBookByCompanyInputModel
    {
        [BindRequired]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }

        [Required]
        [Display(Name = "Start Date - (dd/mm/yyyy)")]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "End Date - (dd/mm/yyyy)")]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string EndDate { get; set; }
    }
}
