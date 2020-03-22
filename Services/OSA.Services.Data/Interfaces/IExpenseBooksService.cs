namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IExpenseBooksService
    {
        Task AddAsync(string startDate, string endDate, string date, int companyId);

        Task<List<ProductionInvoice>> GetAllProductionInvoicesByMonthAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<List<Receipt>> GetAllReceiptsByMonthAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<List<BookValue>> GetAllBookValuesByMonthAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<List<Sell>> GetAllSalesByMonthAsync(DateTime startDate, DateTime endDate, int companyId);
    }
}
