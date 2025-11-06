using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIVIT.CIPA.Api.Domain.Resources;

namespace TIVIT.CIPA.Api.Domain.Extensions
{
    internal static class IntegerExtension
    {
        public static string GetMonthLocalizeDescription(this int value, string language)
        {
            var cultureInfo = language == "pt" ? new CultureInfo("pt-BR") : new CultureInfo("es-ES");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(value);

            return string.Concat(monthName[..1].ToUpper(), monthName[1..]);
        }
    }
}
