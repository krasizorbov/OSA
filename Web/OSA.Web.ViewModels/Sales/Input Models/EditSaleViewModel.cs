namespace OSA.Web.ViewModels.Sales.Input_Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;

    public class EditSaleViewModel
    {
        private const string DisplayTotalPrice = "Тотална Цена";
        private const string DisplayProfitPercent = "Процент Печалба";

        [Required]
        public int Id { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        [Display(Name = DisplayTotalPrice)]
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
