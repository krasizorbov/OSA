namespace OSA.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;
    using OSA.Data.Common.Models;

    public class ExpenseBook : BaseDeletableModel<int>
    {
        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalExternalCost { get; set; } // This comes from Production Invoice

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalSalaryCost { get; set; } // This comes from Receipts

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal TotalBookValue { get; set; } // This comes from BookValues

        public DateTime Date { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Profit { get; set; } // This comes from Sells

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public decimal TotalCost => this.TotalSalaryCost + this.TotalExternalCost + this.TotalBookValue;

        public decimal TotalProfit => this.Profit - this.TotalCost;
    }
}
