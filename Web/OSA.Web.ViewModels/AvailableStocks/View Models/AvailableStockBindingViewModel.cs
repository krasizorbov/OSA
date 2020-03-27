namespace OSA.Web.ViewModels.AvailableStocks.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class AvailableStockBindingViewModel
    {
        public string Name { get; set; }

        public IEnumerable<AvailableStock> AvailableStocks { get; set; }
    }
}
