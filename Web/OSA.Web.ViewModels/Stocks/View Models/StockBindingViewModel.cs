namespace OSA.Web.ViewModels.Stocks.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class StockBindingViewModel
    {
        public string Name { get; set; }

        public IEnumerable<Stock> Stocks { get; set; }
    }
}
