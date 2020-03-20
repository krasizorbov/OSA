namespace OSA.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "OSA";

        public const string AdministratorRoleName = "Administrator";

        public const int SupplierNameMaxLength = 40;

        public const int CompanyNameMaxLength = 40;

        public const int StockNameMaxLength = 20;

        public const int BulstatMinLength = 9;

        public const int BulstatMaxLength = 10;

        public const double DecimalMinValue = 0.00;

        public const double DecimalMaxValue = double.MaxValue;

        public const int ProfitMinValue = 100;

        public const int ProfitMaxValue = 150;

        public const int InvoiceNumberMaxLength = 15;

        public const int ReceiptNumberMaxLength = 15;

        public const string InvalidBulstat = "A Valid Bulstat Contains Only Numbers!";

        public const string ValidInvoiceNumber = "A Valid Invoice Number Contains Only Numbers!";

        public const string ValidReceiptNumber = "A Valid Receipt Number Contains Only Numbers!";

        public const string InvalidDateTimeFormat = "This is not a valid date!";

        public const string CompanyErrorMessage = "Please register a company before proceeding!";
    }
}
