namespace OSA.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;
    using OSA.Data.Common.Models;

    public class Stock : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.StockNameMaxLength)]
        public string Name { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Quantity { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
