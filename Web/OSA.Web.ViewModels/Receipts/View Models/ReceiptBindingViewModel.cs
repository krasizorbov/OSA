namespace OSA.Web.ViewModels.Receipts.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class ReceiptBindingViewModel
    {
        public string Name { get; set; }

        public IEnumerable<Receipt> Receipts { get; set; }
    }
}
