using System;
using System.Collections.Generic;
using System.Text;
using Taxi_Manager.Domain.Enums;

namespace Taxi_Manager.Helpers
{
    public static class DateTimeHelper
    {
        public static LicenceStatus GetLicenceStatus(DateTime licenceExpirationDate, int monthsUntilWarning = 3)
        {
            if (licenceExpirationDate < DateTime.Now)
            {
                return LicenceStatus.Expiered;
            }
            else if (licenceExpirationDate <= DateTime.Now.AddMonths(monthsUntilWarning))
            {
                return LicenceStatus.NearExpiration;
            }
            else
            {
                return LicenceStatus.Valid;
            }
        }
    }
}
