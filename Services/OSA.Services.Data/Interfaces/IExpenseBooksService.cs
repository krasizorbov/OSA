namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IExpenseBooksService
    {
        Task AddAsync(string startDate, string endDate, int companyId);

        Task<List<ProductionInvoice>> GetAllProductionInvoicesByMonthAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<List<Receipt>> GetAllReceiptsByMonthAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<List<Sale>> GetAllSalesByMonthAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<ExpenseBook> ExpenseBookExistAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<IEnumerable<ExpenseBook>> GetExpenseBooksByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<AvailableStock> GetMonthlyAvailableStockByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<ExpenseBook> DeleteAsync(int id);

        Task<ExpenseBook> GetExpenseBookByIdAsync(int id);
    }
}
