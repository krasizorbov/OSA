﻿namespace OSA.Services.Data
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class ReceiptsService : IReceiptsService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<Receipt> receiptsRepository;
        private readonly ApplicationDbContext context;

        public ReceiptsService(IDeletableEntityRepository<Receipt> receiptsRepository, ApplicationDbContext context)
        {
            this.receiptsRepository = receiptsRepository;
            this.context = context;
        }

        public async Task AddAsync(string receiptNumber, string date, decimal salary, int companyId)
        {
            var receipt = new Receipt
            {
                ReceiptNumber = receiptNumber,
                Date = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture),
                Salary = salary,
                CompanyId = companyId,
            };
            await this.receiptsRepository.AddAsync(receipt);
            await this.receiptsRepository.SaveChangesAsync();
        }

        public async Task<string> ReceiptExistAsync(string receiptNumber, int companyId)
        {
            var number = await this.context.Receipts
                .Where(x => x.CompanyId == companyId && x.ReceiptNumber == receiptNumber)
                .Select(x => x.ReceiptNumber)
                .FirstOrDefaultAsync();

            return number;
        }
    }
}
