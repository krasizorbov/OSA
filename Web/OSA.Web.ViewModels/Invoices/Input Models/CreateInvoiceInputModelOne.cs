namespace OSA.Web.ViewModels.Invoices.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Common;

    public class CreateInvoiceInputModelOne
    {
        [BindRequired]
        [Display(Name = GlobalConstants.DisplayCompany)]
        [Required(ErrorMessage = GlobalConstants.FieldIsRequired)]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }
    }
}
