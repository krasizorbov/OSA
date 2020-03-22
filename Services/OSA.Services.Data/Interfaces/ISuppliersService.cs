﻿namespace OSA.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ISuppliersService
    {
        Task AddAsync(string name, string bulstat, int companyId);

        Task<ICollection<SelectListItem>> GetAllSuppliersByCompanyIdAsync(int companyId);

        bool SupplierExist(string supplierName, int companyId);
    }
}