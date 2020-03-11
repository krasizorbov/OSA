namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISuppliersService
    {
        Task AddAsync(string name, string bulstat, int companyId);
    }
}
