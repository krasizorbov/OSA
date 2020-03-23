namespace OSA.Web.ViewModels.Companies.View_Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OSA.Data.Models;

    public class CompanyBindingViewModel
    {
        public IEnumerable<Company> Companies { get; set; }
    }
}
