﻿namespace OSA.Web.ViewModels.Suppliers.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreateSupplierInputModel
    {
        private const string BulstatFormat = "[0-9]*";
        private const string DisplaySupplier = "Доставчик";
        private const string DisplayCompany = "Фирма";
        private const string DisplayBulstat = "Булстат";
        private const string RequiredSupplier = "Доставчика е задължителен!";
        private const string RequiredBulstat = "Булстата е задължителен!";
        private const string RequiredCompany = "Фирмата е задължителна!";

        [Display(Name = DisplaySupplier)]
        [Required(ErrorMessage = RequiredSupplier)]
        [MaxLength(GlobalConstants.CompanyNameMaxLength)]
        public string Name { get; set; }

        [Display(Name = DisplayBulstat)]
        [Required(ErrorMessage = RequiredBulstat)]
        [MinLength(GlobalConstants.BulstatMinLength)]
        [MaxLength(GlobalConstants.BulstatMaxLength)]
        [RegularExpression(BulstatFormat, ErrorMessage = GlobalConstants.InvalidBulstat)]
        public string Bulstat { get; set; }

        [BindRequired]
        [Display(Name = DisplayCompany)]
        [Required(ErrorMessage = RequiredCompany)]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }
    }
}
