namespace OSA.Web.ViewModels.Companies.Input_Models
{
    using System.ComponentModel.DataAnnotations;

    using OSA.Common;

    public class CreateCompanyInputModel
    {
        [Required]
        [Display(Name = "Company Name")]
        [MaxLength(GlobalConstants.CompanyNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Bulstat")]
        [MinLength(GlobalConstants.BulstatMinLength)]
        [MaxLength(GlobalConstants.BulstatMaxLength)]
        [RegularExpression("[0-9]*", ErrorMessage = GlobalConstants.ValidBulstat)]
        public string Bulstat { get; set; }
    }
}
