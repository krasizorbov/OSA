namespace OSA.Web.ViewModels.Purchases.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class PurchaseBindingViewModel
    {
        public string Name { get; set; }

        public IEnumerable<Purchase> Purchases { get; set; }
    }
}
