namespace OSA.Web.ViewModels.Sells.Input_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreateSellInputModelTwo
    {
        private const string DateTimeFormat = @"[0-9]{2}\/[0-9]{2}\/[0-9]{4}";

        [BindRequired]
        [Required(ErrorMessage = "Please select a Stock")]
        [Display(Name = "Stock")]
        [MaxLength(GlobalConstants.StockNameMaxLength)]
        public string StockName { get; set; }

        public IEnumerable<string> StockNames { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalPrice { get; set; }

        [Range(GlobalConstants.ProfitMinValue, GlobalConstants.ProfitMaxValue)]
        public int ProfitPercent { get; set; }

        [Required]
        [Display(Name = "End date of the month - (dd/mm/yyyy)")]
        [RegularExpression(DateTimeFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string Date { get; set; }
    }
}
