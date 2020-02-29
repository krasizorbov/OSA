namespace OSA.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;
    using OSA.Data.Common.Models;

    public class Purchase : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.StockNameMaxLength)]
        public string StockName { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalQuantity { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public decimal AveragePrice => this.TotalPrice / this.TotalQuantity;
    }
}
