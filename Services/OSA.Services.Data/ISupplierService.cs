namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISupplierService
    {
        Task AddAsync(string name, string bulstat);
    }
}
