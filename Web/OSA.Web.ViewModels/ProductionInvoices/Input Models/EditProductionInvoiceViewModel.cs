namespace OSA.Web.ViewModels.ProductionInvoices.Input_Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;

    public class EditProductionInvoiceViewModel
    {
        private const string InvoiceNumberFormat = "0*[0-9]*";
        private const string DisplayInvoiceNumber = "Номер на касов ордер";
        private const string DisplayInvoiceIssueDate = "Дата на издаване - (дд/ММ/гггг)";
        private const string DisplaySalary = "Заплата";
        private const string DisplayExternalCost = "Външни разходи";

        public int Id { get; set; }

        [Display(Name = DisplayInvoiceNumber)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [MaxLength(GlobalConstants.InvoiceNumberMaxLength)]
        [RegularExpression(InvoiceNumberFormat, ErrorMessage = GlobalConstants.ValidInvoiceNumber)]
        public string InvoiceNumber { get; set; }

        [Display(Name = DisplayInvoiceIssueDate)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string Date { get; set; }

        [Display(Name = DisplaySalary)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Salary { get; set; }

        [Display(Name = DisplayExternalCost)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal ExternalCost { get; set; }
    }
}
