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

        public const string InvalidBulstat = "A Valid Bulstat Contains Only Numbers!";

        public const string ValidInvoiceNumber = "A Valid Invoice Contains Only Numbers!";

        public const string ValidReceiptNumber = "A Valid Receipt Contains Only Numbers!";

        public const string InvalidDateTimeFormat = "This is not a valid date format!";

        public const string InvalidDateTime = " This is not a valid date!";

        public const string DateTimeRegexFormat = @"[0-9]{2}\/(0*[1-9]|1[0-2])\/[0-9]{4}";

        public const string CompanyErrorMessage = "Please register a company before proceeding!";

        public const string SupplierErrorMessage = "Please register a supplier before proceeding!";

        public const string InvoiceErrorMessage = "Please register an invoice before proceeding!";

        public const string StockErrorMessage = "Please register a stock before proceeding!";

        public const string DateFormat = "dd/MM/yyyy";

        public const string SuccessfullyRegistered = "Data has been registered successfully!";

        public const string AvailableStockRegisterErrorMessage = "Nothing has been registered! Check your monthly sales or purchases!";
    }
}
