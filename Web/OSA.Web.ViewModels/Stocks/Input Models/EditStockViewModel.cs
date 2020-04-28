namespace OSA.Web.ViewModels.Stocks.Input_Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;

    public class EditStockViewModel
    {
        private const string DisplayStock = "Стока";
        private const string DisplayTotalQuntity = "Количество";
        private const string DisplayTotalPrice = "Цена";
        private const string DisplayDate = "Дата - (дд/ММ/гггг)";

        [Required]
        public int Id { get; set; }

        [Display(Name = DisplayStock)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [MaxLength(GlobalConstants.StockNameMaxLength)]
        public string Name { get; set; }

        [Display(Name = DisplayTotalQuntity)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Quantity { get; set; }

        [Display(Name = DisplayTotalPrice)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Price { get; set; }

        [Display(Name = DisplayDate)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string Date { get; set; }
    }
}
