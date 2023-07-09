using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalcuateAge(this DateOnly dateonly)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - dateonly.Year;

            if (dateonly > today.AddYears(-age)) age--;

            return age;
        }

    }
}