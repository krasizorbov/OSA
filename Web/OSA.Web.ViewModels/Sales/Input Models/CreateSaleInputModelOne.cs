﻿namespace OSA.Web.ViewModels.Sales.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateSaleInputModelOne
    {
        private const string DisplayCompany = "Фирма";
        private const string RequiredCompany = "Фирмата е задължителна";

        [BindRequired]
        [Display(Name = DisplayCompany)]
        [Required(ErrorMessage = RequiredCompany)]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }
    }
}
