namespace OSA.Web.ViewModels.Sales.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class SaleBindingViewModel
    {
        public string Name { get; set; }

        public IEnumerable<Sale> Sales { get; set; }
    }
}
