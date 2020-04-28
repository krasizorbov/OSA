namespace OSA.Web.ViewModels.Companies.Input_Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using OSA.Common;

    [BindRequired]
    public class CreateCompanyInputModel
    {
        private const string BulstatFormat = "[0-9]*";

        [Display(Name = GlobalConstants.DisplayCompany)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [MaxLength(GlobalConstants.CompanyNameMaxLength)]
        public string Name { get; set; }

        [Display(Name = GlobalConstants.DisplayBulstat)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [MinLength(GlobalConstants.BulstatMinLength)]
        [MaxLength(GlobalConstants.BulstatMaxLength)]
        [RegularExpression(BulstatFormat, ErrorMessage = GlobalConstants.InvalidBulstat)]
        public string Bulstat { get; set; }
    }
}
