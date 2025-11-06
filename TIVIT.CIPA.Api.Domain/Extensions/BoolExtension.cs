using System.Globalization;
using TIVIT.CIPA.Api.Domain.Resources;

namespace TIVIT.CIPA.Api.Domain.Extensions
{
    internal static class BoolExtension
    {
        public static string GetLocalizeDescription(this bool value, string language)
        {
            language = language == "es" ? "es-ES" : "pt-BR";

            var culture = new CultureInfo(language);

            var rm = BoolDescription.ResourceManager;

            var resourceDisplayName = rm.GetString(value.ToString(), culture);

            return string.IsNullOrWhiteSpace(resourceDisplayName) ? string.Format("[[{0}]]", value) : resourceDisplayName;
        }
    }
}
