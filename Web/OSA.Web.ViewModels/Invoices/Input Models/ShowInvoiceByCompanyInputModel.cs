namespace OSA.Web.ViewModels.Invoices.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class ShowInvoiceByCompanyInputModel
    {
        private const string DisplayCompany = "Фирма";
        private const string DisplayStartDate = "Първи ден от месеца - (дд/ММ/гггг)";
        private const string DisplayEndDate = "Последен ден от месеца - (дд/ММ/гггг)";

        [BindRequired]
        [Display(Name = DisplayCompany)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }

        [Display(Name = DisplayStartDate)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string StartDate { get; set; }

        [Display(Name = DisplayEndDate)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        [RegularExpression(GlobalConstants.DateTimeRegexFormat, ErrorMessage = GlobalConstants.InvalidDateTimeFormat)]
        public string EndDate { get; set; }
    }
}
