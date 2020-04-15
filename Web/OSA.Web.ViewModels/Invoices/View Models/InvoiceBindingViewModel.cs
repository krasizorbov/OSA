namespace OSA.Web.ViewModels.Invoices.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class InvoiceBindingViewModel
    {
        public string Name { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }
}
