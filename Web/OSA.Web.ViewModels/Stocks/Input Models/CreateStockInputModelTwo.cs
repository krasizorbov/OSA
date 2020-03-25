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
        [Required]
        [Display(Name = "Stock")]
        [MaxLength(GlobalConstants.StockNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Total Quantity")]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [Display(Name = "Total Price")]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Invoice issue date - (dd/mm/yyyy)")]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string Date { get; set; }

        [BindRequired]
        [Display(Name = "Invoice")]
        public int InvoiceId { get; set; }

        public IEnumerable<SelectListItem> Invoices { get; set; }
    }
}
