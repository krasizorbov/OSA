namespace OSA.Web.ViewModels.Receipts.Input_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreateReceiptInputModel
    {
        private const string ReceiptNumberFormat = "0*[0-9]*";

        [BindRequired]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }

        [Required]
        [Display(Name = "Receipt Number")]
        [MaxLength(GlobalConstants.ReceiptNumberMaxLength)]
        [RegularExpression(ReceiptNumberFormat, ErrorMessage = GlobalConstants.ValidReceiptNumber)]
        public string ReceiptNumber { get; set; }

        [Required]
        [Display(Name = "End date of the month - (dd/mm/yyyy)")]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string Date { get; set; }

        [Required]
        [Display(Name = "Salary Cost")]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Salary { get; set; }
    }
}