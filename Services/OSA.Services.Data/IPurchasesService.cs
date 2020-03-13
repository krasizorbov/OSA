namespace OSA.Services.Data
{
    using System.Threading.Tasks;

    public interface IPurchasesService
    {
        Task AddAsync(string stockName, decimal totalQuantity, decimal totalPrice, string date);
    }
}
