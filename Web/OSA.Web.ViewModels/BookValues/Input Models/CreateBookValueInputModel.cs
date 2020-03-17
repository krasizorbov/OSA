namespace OSA.Web.ViewModels.BookValues.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreateBookValueInputModel
    {
        private const string DateTimeFormat = @"[0-9]{2}\/[0-9]{2}\/[0-9]{4}";

        [BindRequired]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }

        public string StockName { get; set; }

        [Required]
        [Display(Name = "Start date of the month - (dd/mm/yyyy)")]
        [RegularExpression(DateTimeFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "End date of the month - (dd/mm/yyyy)")]
        [RegularExpression(DateTimeFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string EndDate { get; set; }

        [Required]
        [Display(Name = "End date of the month - (dd/mm/yyyy)")]
        [RegularExpression(DateTimeFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string Date { get; set; }
    }
}
