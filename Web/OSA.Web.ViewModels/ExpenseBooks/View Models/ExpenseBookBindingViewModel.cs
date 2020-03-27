namespace OSA.Web.ViewModels.ExpenseBooks.View_Models
{
    using System.Collections.Generic;

    using OSA.Data.Models;

    public class ExpenseBookBindingViewModel
    {
        public string Name { get; set; }

        public IEnumerable<ExpenseBook> ExpenseBooks { get; set; }
    }
}
