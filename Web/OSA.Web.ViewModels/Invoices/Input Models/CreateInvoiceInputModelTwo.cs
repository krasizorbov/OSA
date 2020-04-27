namespace OSA.Web.ViewModels.Invoices.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreateInvoiceInputModelTwo
    {
        private const string InvoiceNumberFormat = "0*[0-9]*";
        private const string DisplayNumber = "Номер на Фактура";
        private const string RequiredNumber = "Номера на фактурата е задължителен!";
        private const string DisplayDate = "Дата на издаване на фактурата - (дд/ММ/гггг)";
        private const string RequiredDate = "Датата е задължителна!";
        private const string DisplayAmount = "Парична стойност";
        private const string RequiredAmount = "Сумата е задължителна!";
        private const string DisplaySupplier = "Доставчик";
        private const string RequiredSupplier = "Доставчика е задължителен!";

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

        [BindRequired]
        [Display(Name = DisplaySupplier)]
        [Required(ErrorMessage = RequiredSupplier)]
        public int SupplierId { get; set; }

        public IEnumerable<SelectListItem> SupplierNames { get; set; }
    }
}
