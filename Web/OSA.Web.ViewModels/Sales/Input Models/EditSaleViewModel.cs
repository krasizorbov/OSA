namespace OSA.Web.ViewModels.Sales.Input_Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;

    public class EditSaleViewModel
    {
        private const string DisplayName = "Име на стока";
        private const string RequiredName = "Стоката е задължителна!";
        private const string DisplayStartDate = "Начална Дата на месеца - (дд/ММ/гггг)";
        private const string RequiredStartDate = "Датата е задължителна!";

        [Required]
        public int Id { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalPrice { get; set; }

        [Range(GlobalConstants.ProfitMinValue, GlobalConstants.ProfitMaxValue)]
        public int ProfitPercent { get; set; }

        [Display(Name = DisplayStartDate)]
        [Required(ErrorMessage = RequiredStartDate)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string StartDate { get; set; }
    }
}
