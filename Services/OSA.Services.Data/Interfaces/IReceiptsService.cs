namespace OSA.Services.Data
{
    using System.Threading.Tasks;

    public interface IReceiptsService
    {
        Task AddAsync(string receiptNumber, string date, decimal salary, int companyId);

        Task<string> ReceiptExistAsync(string receiptNumber, int companyId);
    }
}
