namespace OSA.Web.ViewModels.Sales.Input_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using OSA.Common;

    public class CreateSaleInputModelTwo
    {
        private const string DisplayTotalprice = "Тотална Цена";
        private const string DisplayProfitPercent = "Процент Печалба";

        [BindRequired]
        [Display(Name = GlobalConstants.DisplayStock)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [MaxLength(GlobalConstants.StockNameMaxLength)]
        public string StockName { get; set; }

        public List<string> StockNames { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        [Display(Name = DisplayTotalprice)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        public decimal TotalPrice { get; set; }

        [Range(GlobalConstants.ProfitMinValue, GlobalConstants.ProfitMaxValue)]
        [Display(Name = DisplayProfitPercent)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        public int ProfitPercent { get; set; }

        [Display(Name = GlobalConstants.DisplayStartDate)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string StartDate { get; set; }
    }
}
