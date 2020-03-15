namespace OSA.Services.Data
{
    using System.Threading.Tasks;

    public interface IBooksValue
    {
        Task AddAsync(decimal price, string stockName, string date, int companyId);
    }
}
