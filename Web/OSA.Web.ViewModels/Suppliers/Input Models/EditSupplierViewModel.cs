namespace OSA.Web.ViewModels.Suppliers.Input_Models
{
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;

    public class EditSupplierViewModel
    {
        private const string BulstatFormat = "[0-9]*";
        private const string DisplaySupplier = "Доставчик";
        private const string DisplayBulstat = "Булстат";
        private const string RequiredName = "Доставчика е задължителен!";
        private const string RequiredBulstat = "Булстата е задължителен!";

        [Required]
        public int Id { get; set; }

        [Display(Name = DisplaySupplier)]
        [Required(ErrorMessage = RequiredName)]
        [MaxLength(GlobalConstants.CompanyNameMaxLength)]
        public string Name { get; set; }

        [Display(Name = DisplayBulstat)]
        [Required(ErrorMessage = RequiredBulstat)]
        [MinLength(GlobalConstants.BulstatMinLength)]
        [MaxLength(GlobalConstants.BulstatMaxLength)]
        [RegularExpression(BulstatFormat, ErrorMessage = GlobalConstants.InvalidBulstat)]
        public string Bulstat { get; set; }
    }
}
