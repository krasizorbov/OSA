namespace OSA.Web.ViewModels.Suppliers.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class SupplierBindingViewModel
    {
        public string Name { get; set; }

        public IEnumerable<Supplier> Suppliers { get; set; }
    }
}
