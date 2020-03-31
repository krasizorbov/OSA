namespace OSA.Web.ViewModels.Sales.Input_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using OSA.Common;

    public class CreateSaleInputModelTwo
    {
        [BindRequired]
        [Required(ErrorMessage = "Please select a Stock")]
        [Display(Name = "Stock")]
        [MaxLength(GlobalConstants.StockNameMaxLength)]
        public string StockName { get; set; }

        public List<string> StockNames { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalPrice { get; set; }

        [Range(GlobalConstants.ProfitMinValue, GlobalConstants.ProfitMaxValue)]
        public int ProfitPercent { get; set; }

        [Required]
        [Display(Name = "Start date of the month - (dd/mm/yyyy)")]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string StartDate { get; set; }
    }
}
