namespace OSA.Web.ViewModels.Sales.Input_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using OSA.Common;

    public class CreateSaleInputModelTwo
    {
        private const string DisplayStock = "Стока";
        private const string RequiredStock = "Моля изберете стока";
        private const string DisplayTotalprice = "Тотална Цена";
        private const string RequiredTotalprice = "Цената е задължителна";
        private const string DisplayProfitPercent = "Процент Печалба";
        private const string RequiredProfitPercent = "Процента печалба е задължителен";
        private const string DisplayStartDate = "Начална дата от месеца - (дд/ММ/гггг)";
        private const string RequiredStartDate = "Датата е задължителна";

        [BindRequired]
        [Display(Name = DisplayStock)]
        [Required(ErrorMessage = RequiredStock)]
        [MaxLength(GlobalConstants.StockNameMaxLength)]
        public string StockName { get; set; }

        public List<string> StockNames { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        [Display(Name = DisplayTotalprice)]
        [Required(ErrorMessage = RequiredTotalprice)]
        public decimal TotalPrice { get; set; }

        [Range(GlobalConstants.ProfitMinValue, GlobalConstants.ProfitMaxValue)]
        [Display(Name = DisplayProfitPercent)]
        [Required(ErrorMessage = RequiredProfitPercent)]
        public int ProfitPercent { get; set; }

        [Display(Name = DisplayStartDate)]
        [Required(ErrorMessage = RequiredStartDate)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string StartDate { get; set; }
    }
}
