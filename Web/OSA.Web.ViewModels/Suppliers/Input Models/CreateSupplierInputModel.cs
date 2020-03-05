namespace OSA.Web.ViewModels.Suppliers.Input_Models
{
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;

    public class CreateSupplierInputModel
    {
        [Required]
        [MaxLength(GlobalConstants.CompanyNameMaxLength)]
        [Display(Name = "Supplier Name")]
        public string Name { get; set; }

        [Required]
        [MinLength(GlobalConstants.BulstatMinLength)]
        [MaxLength(GlobalConstants.BulstatMaxLength)]
        [RegularExpression("[0-9]*", ErrorMessage = GlobalConstants.ValidBulstat)]
        [Display(Name = "Bulstat")]
        public string Bulstat { get; set; }
    }
}
