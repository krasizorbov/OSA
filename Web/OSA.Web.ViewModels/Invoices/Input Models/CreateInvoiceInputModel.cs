namespace OSA.Web.ViewModels.Invoices.Input_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreateInvoiceInputModel
    {
        [Required]
        [Display(Name = "Invoice Number")]
        [MaxLength(GlobalConstants.InvoiceNumberMaxLength)]
        [RegularExpression("0*[0-9]*", ErrorMessage = GlobalConstants.ValidInvoiceNumber)]
        public string InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Invoice Issue Date DD/MM/YYYY")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        public IEnumerable<SelectListItem> SupplierNames { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }
    }
}
