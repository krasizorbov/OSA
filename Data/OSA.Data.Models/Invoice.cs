namespace OSA.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;
    using OSA.Data.Common.Models;

    public class Invoice : BaseDeletableModel<int>
    {
        public Invoice()
        {
            this.Stocks = new HashSet<Stock>();
        }

        [Required]
        [MaxLength(GlobalConstants.InvoiceNumberMaxLength)]
        public string InvoiceNumber { get; set; }

        public DateTime Date { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public ICollection<Stock> Stocks { get; set; }
    }
}
