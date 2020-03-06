namespace OSA.Services.Data
{
    using System.Threading.Tasks;

    public interface ICompanyService
    {
        Task AddAsync(string name, string bulstat);
    }
}
