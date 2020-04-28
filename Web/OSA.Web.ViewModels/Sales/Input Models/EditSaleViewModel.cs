namespace OSA.Web.ViewModels.Sales.Input_Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;

    public class EditSaleViewModel
    {
        private const string DisplayTotalPrice = "Тотална Цена";
        private const string RequiredTotalPrice = "Цената е задължителна!";
        private const string DisplayProfitPercent = "Процент Печалба";
        private const string RequiredProfitPercent = "Процента печалба е задължителен!";
        private const string DisplayStartDate = "Първи ден от месеца - (дд/ММ/гггг)";
        private const string RequiredStartDate = "Датата е задължителна!";

        [Required]
        public int Id { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        [Display(Name = DisplayTotalPrice)]
        [Required(ErrorMessage = RequiredTotalPrice)]
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
