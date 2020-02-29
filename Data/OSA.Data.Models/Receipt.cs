namespace OSA.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;
    using OSA.Data.Common.Models;

    public class Receipt : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.ReceiptNumberMaxLength)]
        public int ReceiptNumber { get; set; }

        [Range(GlobalConstants.DecimalMinValue, GlobalConstants.DecimalMaxValue)]
        public decimal Salary { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
