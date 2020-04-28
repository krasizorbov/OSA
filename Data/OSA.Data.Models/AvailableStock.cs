namespace OSA.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;
    using OSA.Data.Common.Models;

    public class AvailableStock : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.StockNameMaxLength)]
        public string StockName { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalPurchasedAmount { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalPurchasedPrice { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalSoldPrice { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public string BookValue { get; set; }

        [Required]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public string AveragePrice { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public decimal TotalSoldQuantity => Convert.ToDecimal(this.BookValue) / Convert.ToDecimal(this.AveragePrice);

        public decimal RemainingQuantity => this.TotalPurchasedAmount - this.TotalSoldQuantity;

        public decimal RemainingPrice => this.RemainingQuantity * Convert.ToDecimal(this.AveragePrice);
    }
}
