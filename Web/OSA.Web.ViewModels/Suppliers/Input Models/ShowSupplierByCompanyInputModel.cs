namespace OSA.Web.ViewModels.Suppliers.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ShowSupplierByCompanyInputModel
    {
        private const string DisplayCompany = "Фирма";

        [BindRequired]
        [Display(Name = DisplayCompany)]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }
    }
}
