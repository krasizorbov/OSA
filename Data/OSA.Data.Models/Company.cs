namespace OSA.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;
    using OSA.Data.Common.Models;

    public class Company : BaseDeletableModel<int>
    {
        public Company()
        {
            this.Invoices = new HashSet<Invoice>();
            this.Suppliers = new HashSet<Supplier>();
            this.Receipts = new HashSet<Receipt>();
            this.ProductionInvoices = new HashSet<ProductionInvoice>();
        }

        [Required]
        [MaxLength(GlobalConstants.CompanyNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(GlobalConstants.BulstatMinLength)]
        [MaxLength(GlobalConstants.BulstatMaxLength)]
        public string Bulstat { get; set; }

        [Required]

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<Invoice> Invoices { get; set; }

        public ICollection<Supplier> Suppliers { get; set; }

        public ICollection<Receipt> Receipts { get; set; }

        public ICollection<ProductionInvoice> ProductionInvoices { get; set; }
    }
}
