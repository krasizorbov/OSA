namespace OSA.Services.Data
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class ReceiptsService : IReceiptsService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<Receipt> receiptsRepository;

        public ReceiptsService(IDeletableEntityRepository<Receipt> receiptsRepository)
        {
            this.receiptsRepository = receiptsRepository;
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
    }
}
