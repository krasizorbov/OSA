﻿namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IPurchasesService
    {
        Task AddAsync(string startDate, string endDate, int companyId);

        Task<List<string>> GetStockNamesForCurrentMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<List<string>> GetStockNamesForPrevoiusMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<List<string>> GetStockNamesAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<decimal> QuantitySoldAsync(string stockName, DateTime startDate, DateTime endDate, int companyId);

        Task<decimal> QuantityPurchasedAsync(string stockName, DateTime startDate, DateTime endDate, int companyId);

        decimal TotalQuantity(string stockName, DateTime startDate, DateTime endDate, int companyId);

        decimal TotalPrice(string stockName, DateTime startDate, DateTime endDate, int companyId);

        Task<List<string>> PurchaseExistAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<ICollection<Purchase>> GetPurchasesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<decimal> GetAvailableStockForPreviousMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, string name, int companyId);

        Task<List<Purchase>> DeleteAsync(List<int> ids);
    }
}
