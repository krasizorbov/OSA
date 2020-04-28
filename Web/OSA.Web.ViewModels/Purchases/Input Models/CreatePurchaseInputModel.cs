namespace OSA.Web.ViewModels.Purchases.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreatePurchaseInputModel
    {
        private const string DisplayCompany = "Фирма";
        private const string RequiredCompany = "Фирмата е задължителна";
        private const string DisplayStartDate = "Първи ден от месеца - (дд/ММ/гггг)";
        private const string RequiredStartDate = "Датата е задължителна";
        private const string DisplayEndDate = "Последен ден от месеца - (дд/ММ/гггг)";
        private const string RequiredEndDate = "Датата е задължителна";

        [BindRequired]
        [Display(Name = DisplayCompany)]
        [Required(ErrorMessage = RequiredCompany)]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }

        [Display(Name = DisplayStartDate)]
        [Required(ErrorMessage = RequiredStartDate)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string StartDate { get; set; }

        [Display(Name = DisplayEndDate)]
        [Required(ErrorMessage = RequiredEndDate)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string EndDate { get; set; }
    }
}
