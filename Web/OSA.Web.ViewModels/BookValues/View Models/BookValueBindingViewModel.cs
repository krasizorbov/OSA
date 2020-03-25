namespace OSA.Web.ViewModels.BookValues.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class BookValueBindingViewModel
    {
        public string Name { get; set; }

        public IEnumerable<BookValue> BookValues { get; set; }
    }
}
