namespace OSA.Web.ViewModels.Suppliers.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class SupplierBindingViewModel
    {
        public IEnumerable<Supplier> Suppliers { get; set; }
    }
}
