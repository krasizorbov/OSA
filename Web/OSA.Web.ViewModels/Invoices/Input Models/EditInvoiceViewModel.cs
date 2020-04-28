﻿namespace OSA.Web.ViewModels.Invoices.Input_Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;

    public class EditInvoiceViewModel
    {
        private const string InvoiceNumberFormat = "0*[0-9]*";
        private const string DisplayNumber = "Номер на Фактура";
        private const string RequiredNumber = "Номера на фактурата е задължителен!";
        private const string DisplayDate = "Дата на издаване на фактурата - (дд/ММ/гггг)";
        private const string RequiredDate = "Датата е задължителна!";
        private const string DisplayAmount = "Парична стойност";
        private const string RequiredAmount = "Сумата е задължителна!";

        [Required]
        public int Id { get; set; }

        [Display(Name = DisplayNumber)]
        [Required(ErrorMessage = RequiredNumber)]
        [MaxLength(GlobalConstants.InvoiceNumberMaxLength)]
        [RegularExpression(InvoiceNumberFormat, ErrorMessage = GlobalConstants.ValidInvoiceNumber)]
        public string InvoiceNumber { get; set; }

        [Display(Name = DisplayDate)]
        [Required(ErrorMessage = RequiredDate)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string Date { get; set; }

        [Display(Name = DisplayAmount)]
        [Required(ErrorMessage = RequiredAmount)]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalAmount { get; set; }
    }
}