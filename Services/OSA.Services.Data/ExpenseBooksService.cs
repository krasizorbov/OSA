namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class ExpenseBooksService : IExpenseBooksService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<ExpenseBook> expenseBooksRepository;
        private readonly ApplicationDbContext context;

        public ExpenseBooksService(IDeletableEntityRepository<ExpenseBook> expenseBooksRepository, ApplicationDbContext context)
        {
            this.expenseBooksRepository = expenseBooksRepository;
            this.context = context;
        }

        public Task AddAsync(string startDate, string endDate, string date, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BookValue>> GetAllBookValuesByMonthAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductionInvoice>> GetAllProductionInvoicesByMonthAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Receipt>> GetAllReceiptsByMonthAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Sell>> GetAllSellsByMonthAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            throw new NotImplementedException();
        }
    }
}
