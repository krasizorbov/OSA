namespace OSA.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;
    using OSA.Data.Common.Models;

    public class ProductionInvoice : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.InvoiceNumberMaxLength)]
        public string InvoiceNumber { get; set; }

        public DateTime Date { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal StockCost { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal ExternalCost { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
