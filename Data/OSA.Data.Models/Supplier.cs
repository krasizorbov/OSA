namespace OSA.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;
    using OSA.Data.Common.Models;

    public class Supplier : BaseDeletableModel<int>
    {
        public Supplier()
        {
            this.Invoices = new HashSet<Invoice>();
        }

        [Required]
        [MaxLength(GlobalConstants.SupplierNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(GlobalConstants.BulstatMinLength)]
        [MaxLength(GlobalConstants.BulstatMaxLength)]
        public string Bulstat { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }
}
