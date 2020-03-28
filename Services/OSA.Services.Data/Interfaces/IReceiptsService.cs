namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IReceiptsService
    {
        Task AddAsync(string receiptNumber, string date, decimal salary, int companyId);

        Task<string> ReceiptExistAsync(string receiptNumber, int companyId);

        Task<IEnumerable<Receipt>> GetReceiptsByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);
    }
}
