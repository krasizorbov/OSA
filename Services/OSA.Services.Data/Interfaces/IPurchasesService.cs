﻿namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPurchasesService
    {
        Task AddAsync(string startDate, string endDate, string date, int companyId);

        Task<List<string>> GetStockNamesForCurrentMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<List<string>> GetStockNamesForPrevoiusMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<List<string>> GetStockNamesAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<decimal> QuantitySoldAsync(string stockName, DateTime startDate, DateTime endDate, int companyId);

        Task<decimal> QuantityPurchasedAsync(string stockName, DateTime startDate, DateTime endDate, int companyId);

        decimal TotalQuantity(string stockName, DateTime startDate, DateTime endDate, int companyId);

        decimal TotalPrice(string stockName, DateTime startDate, DateTime endDate, int companyId);

        Task<List<string>> PurchaseExistAsync(DateTime startDate, DateTime endDate, int companyId);
    }
}