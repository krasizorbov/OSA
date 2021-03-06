﻿namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Data.Models;

    public interface IInvoicesService
    {
        Task AddAsync(string invoiceNumber, decimal totalAmount, string date, int supplierId, int companyId);

        Task<ICollection<SelectListItem>> GetAllInvoicesByCompanyIdAsync(int companyId);

        Task<string> InvoiceExistAsync(string invoiceNumber, int companyId);

        Task<ICollection<Invoice>> GetInvoicesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<string> GetInvoiceNumberByInvoiceIdAsync(int invoiceId);

        Task<Invoice> DeleteAsync(int id);

        Task<Invoice> GetInvoiceByIdAsync(int id);

        bool CompanyIdExists(int id);

        Task UpdateInvoice(int id, string number, decimal totalAmount, DateTime date);
    }
}
