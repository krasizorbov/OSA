﻿namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IAvailableStocksService
    {
        Task AddAsync(string startDate, string endDate, int companyId);

        Task<List<string>> GetPurchasedStockNamesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<List<string>> GetSoldStockNamesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<Purchase> GetCurrentPurchasedStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int companyId);

        Task<Sale> GetCurrentSoldStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int companyId);

        Task<decimal> GetCurrentBookValueForStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int companyId);

        Task<List<string>> AvailableStockExistAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<IEnumerable<AvailableStock>> GetAvailableStocksByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);
    }
}
