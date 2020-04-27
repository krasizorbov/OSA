namespace OSA.Web.ViewModels.Companies.Input_Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using OSA.Common;

    [BindRequired]
    public class CreateCompanyInputModel
    {
        private const string BulstatFormat = "[0-9]*";
        private const string DisplayCompany = "Фирма";
        private const string DisplayBulstat = "Булстат";
        private const string RequiredName = "Фирмата е задължителна!";
        private const string RequiredBulstat = "Булстата е задължителен!";

        [Display(Name = DisplayCompany)]
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
