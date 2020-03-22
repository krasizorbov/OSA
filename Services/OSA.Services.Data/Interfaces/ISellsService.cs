namespace OSA.Services.Data
{
    using System;
    using System.Threading.Tasks;

    public interface ISellsService
    {
        Task AddAsync(string stockName, decimal totalPrice, int profitPercent, string date, int companyId);

        Task<string> SaleExistAsync(string stockName, int companyId);
    }
}
