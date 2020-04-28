namespace OSA.Web.ViewModels.Stocks.Input_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreateStockInputModelTwo
    {
        private const string DisplayStock = "Стока";
        private const string RequiredStock = "Стоката е задължителна";
        private const string DisplayTotalQuntity = "Количество";
        private const string RequiredTotalQuantity = "Количеството е задължително";
        private const string DisplayTotalPrice = "Цена";
        private const string RequiredTotalPrice = "Цената е задължителна";
        private const string DisplayDate = "Дата - (дд/ММ/гггг)";
        private const string RequiredDate = "Датата е задължителна";
        private const string DisplayInvoice = "Фактура";
        private const string RequiredInvoice = "Фактурата е задължителна";

        [Display(Name = DisplayStock)]
        [Required(ErrorMessage = RequiredStock)]
        [MaxLength(GlobalConstants.StockNameMaxLength)]
        public string Name { get; set; }

        [Display(Name = DisplayTotalQuntity)]
        [Required(ErrorMessage = RequiredTotalQuantity)]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Quantity { get; set; }

        [Display(Name = DisplayTotalPrice)]
        [Required(ErrorMessage = RequiredTotalPrice)]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Price { get; set; }

        [Display(Name = DisplayDate)]
        [Required(ErrorMessage = RequiredDate)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string Date { get; set; }

        [BindRequired]
        [Display(Name = DisplayInvoice)]
        [Required(ErrorMessage = RequiredInvoice)]
        public int InvoiceId { get; set; }

        public IEnumerable<SelectListItem> Invoices { get; set; }
    }
}
