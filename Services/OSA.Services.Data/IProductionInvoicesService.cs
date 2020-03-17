namespace OSA.Services.Data
{
    using System.Threading.Tasks;

    public interface IProductionInvoicesService
    {
        Task AddAsync(string invoiceNumber, string date, decimal materialCost, decimal externalCostint, int companyId);
    }
}
