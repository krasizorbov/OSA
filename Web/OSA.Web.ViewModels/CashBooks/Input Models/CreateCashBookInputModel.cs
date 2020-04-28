namespace OSA.Web.ViewModels.CashBooks.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreateCashBookInputModel
    {
        private const string DisplaySaldo = "Салдо";
        private const string DisplayOwnFunds = "Лични Средства";

        [BindRequired]
        [Display(Name = GlobalConstants.DisplayCompany)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }

        [Display(Name = DisplaySaldo)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Saldo { get; set; }

        [Display(Name = DisplayOwnFunds)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal OwnFunds { get; set; }

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
