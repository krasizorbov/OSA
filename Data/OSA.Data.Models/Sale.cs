namespace OSA.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;
    using OSA.Data.Common.Models;

    public class Sale : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.StockNameMaxLength)]
        public string StockName { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalPrice { get; set; }

        [Range(GlobalConstants.ProfitMinValue, GlobalConstants.ProfitMaxValue)]
        public int ProfitPercent { get; set; }

        [Required]
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public string AveragePrice { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public decimal BookValue => this.TotalPrice / ((decimal)this.ProfitPercent / 100);

        public decimal TotalPurchaseQuantity => this.BookValue / Convert.ToDecimal(this.AveragePrice);
    }
}
