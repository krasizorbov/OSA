namespace OSA.Web.ViewModels.Suppliers.Input_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using OSA.Common;

    public class CreateSupplierInputModel
    {
        [Required]
        [MaxLength(GlobalConstants.CompanyNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(GlobalConstants.BulstatMinLength)]
        [MaxLength(GlobalConstants.BulstatMaxLength)]
        public string Bulstat { get; set; }
    }
}
