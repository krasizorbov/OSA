namespace OSA.Web.ViewModels.Sells.Input_Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateSellInputModelOne
    {
        [BindRequired]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames { get; set; }
    }
}
