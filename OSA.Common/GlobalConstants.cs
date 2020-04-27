namespace OSA.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "OSA";

        public const string AdministratorRoleName = "Administrator";

        public const int SupplierNameMaxLength = 80;

        public const int CompanyNameMaxLength = 80;

        public const int StockNameMaxLength = 50;

        public const int BulstatMinLength = 9;

        public const int BulstatMaxLength = 10;

        public const double DecimalMinValue = 0.00;

        public const double DecimalMaxValue = double.MaxValue;

        public const int ProfitMinValue = 100;

        public const int ProfitMaxValue = 150;

        public const int InvoiceNumberMaxLength = 15;

        public const int ReceiptNumberMaxLength = 15;

        public const string InvalidBulstat = "Невалиден Булстат!";

        public const string ValidInvoiceNumber = "Невалиден Номер на Фактура!";

        public const string InvalidDateTimeFormat = "Невалиден Формат на Датата!";

        public const string InvalidDateTime = " Невалидна Дата!";

        public const string DateTimeRegexFormat = @"[0-9]{2}\/(0*[1-9]|1[0-2])\/[0-9]{4}";

        public const string CompanyErrorMessage = "Моля регистрирайте фирма преди да продължите!";

        public const string SupplierErrorMessage = "Моля регистрирайте доставчик преди да продължите!";

        public const string InvoiceErrorMessage = "Моля регистрирайте фактура преди да продължите!";

        public const string StockErrorMessage = "Моля регистрирайте стока преди да продължите!";

        public const string PurchaseErrorMessage = "Моля регистрирайте покупка преди да продължите!";

        public const string DateFormat = "dd/MM/yyyy";

        public const string SuccessfullyRegistered = "Данните са регистрирани УСПЕШНО!";

        public const string SuccessfullyDeleted = "Данните са изтрити УСПЕШНО!";

        public const string SuccessfullyUpdated = "Данните са актуализирани УСПЕШНО!";

        public const string AvailableStockRegisterErrorMessage = "Данните не са регистрирани! Няма наличност за предния месец!";
    }
}
