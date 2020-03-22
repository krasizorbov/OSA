﻿namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IBookValuesService
    {
        Task AddAsync(string startDate, string endDate, string date, int companyId);

        Task<List<Sell>> GetMonthlySellsAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<decimal> GetStockMonthlyAveragePriceAsync(string stockName, DateTime startDate, DateTime endDate, int companyId);
    }
}