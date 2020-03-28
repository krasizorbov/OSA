namespace OSA.Web.ViewModels.CashBooks.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class CashBookBindingViewModel
    {
        public string Name { get; set; }

        public IEnumerable<CashBook> CashBooks { get; set; }
    }
}
