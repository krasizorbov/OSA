namespace OSA.Web.ViewModels.Companies.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class CompanyBindingViewModel
    {
        public IEnumerable<Company> Companies { get; set; }
    }
}
