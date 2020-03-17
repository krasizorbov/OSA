﻿namespace OSA.Web.ViewModels.ProductionInvoices.Input_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreateProductionInvoiceInputModel
    {
        private const string DateTimeFormat = @"[0-9]{2}\/[0-9]{2}\/[0-9]{4}";
        private const string InvoiceNumberFormat = "0*[0-9]*";

        [BindRequired]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }

        [Required]
        [Display(Name = "Invoice Number")]
        [MaxLength(GlobalConstants.InvoiceNumberMaxLength)]
        [RegularExpression(InvoiceNumberFormat, ErrorMessage = GlobalConstants.ValidInvoiceNumber)]
        public string InvoiceNumber { get; set; }

        [Required]
        [Display(Name = "Invoice Issue Date - (dd/mm/yyyy)")]
        [RegularExpression(DateTimeFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string Date { get; set; }

        [Required]
        [Display(Name = "Material Cost")]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal StockCost { get; set; }

        [Required]
        [Display(Name = "External Cost")]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal ExternalCost { get; set; }
    }
}