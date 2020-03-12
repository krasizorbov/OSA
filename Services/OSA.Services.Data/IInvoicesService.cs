namespace OSA.Services.Data
{
    using System.Threading.Tasks;

    public interface IInvoicesService
    {
        Task AddAsync(string invoiceNumber, string date, int supplierId, int companyId);
    }
}
