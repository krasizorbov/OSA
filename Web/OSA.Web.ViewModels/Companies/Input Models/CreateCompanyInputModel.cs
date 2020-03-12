namespace OSA.Web.ViewModels.Companies.Input_Models
{
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;

    public class CreateCompanyInputModel
    {
        private const string BulstatFormat = "[0-9]*";

        [Required]
        [Display(Name = "Company")]
        [MaxLength(GlobalConstants.CompanyNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Bulstat")]
        [MinLength(GlobalConstants.BulstatMinLength)]
        [MaxLength(GlobalConstants.BulstatMaxLength)]
        [RegularExpression(BulstatFormat, ErrorMessage = GlobalConstants.InvalidBulstat)]
        public string Bulstat { get; set; }
    }
}
