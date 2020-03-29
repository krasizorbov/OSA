namespace OSA.Services.Data
{
    using System;
    using System.Globalization;

    using OSA.Common;
    using OSA.Services.Data.Interfaces;

    public class DateTimeValidationService : IDateTimeValidationService
    {
        public bool IsValidDateTime(string date)
        {
            if (DateTime.TryParseExact(date, GlobalConstants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
