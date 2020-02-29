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
        public decimal TotalSoldAmount { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal BookValue { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public decimal RemainingAmount => this.TotalPurchasedAmount - this.TotalSoldAmount;

        public decimal RemainingPrice => this.TotalPurchasedPrice - this.BookValue;
    }
}
