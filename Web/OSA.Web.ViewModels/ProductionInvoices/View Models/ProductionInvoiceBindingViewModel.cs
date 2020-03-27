namespace OSA.Web.ViewModels.ProductionInvoices.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class ProductionInvoiceBindingViewModel
    {
        public string Name { get; set; }

        public IEnumerable<ProductionInvoice> ProductionInvoices { get; set; }
    }
}
