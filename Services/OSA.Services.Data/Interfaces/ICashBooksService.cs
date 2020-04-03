﻿namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface ICashBooksService
    {
        Task AddAsync(string startDate, string endDate, int companyId);

        decimal TotalSumStockCost(DateTime startDate, DateTime endDate, int companyId);

        Task<ExpenseBook> GetMonthlyExpenseBook(DateTime startDate, DateTime endDate, int companyId);

        Task<CashBook> CashBookExistAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<IEnumerable<CashBook>> GetCashBooksByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);
    }
}
