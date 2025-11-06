using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using TIVIT.CIPA.Api.Domain.Resources;

namespace TIVIT.CIPA.Api.Domain.Extensions
{
    public static class EnumExtension
    {
        public static string GetLocalizeDescription(this Enum en, string language)
        {
            language = language == "es" ? "es-ES" : "pt-BR";

            var culture = new CultureInfo(language);

            var rm = EnumDescription.ResourceManager;

            var enumName = en.GetType().Name;
            var enumValue = en.ToString();

            var resourceDisplayName = rm.GetString($"{enumName}_{enumValue}", culture);

            return string.IsNullOrWhiteSpace(resourceDisplayName) ? string.Format("[[{0}]]", en) : resourceDisplayName;
        }

        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        public static (int min, int max) GetRange(this Enum value)
        {
            MemberInfo member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            int min = member?.GetCustomAttribute<RangeAttribute>().Min ?? 0;
            int max = member?.GetCustomAttribute<RangeAttribute>().Max ?? 0;

            return (min, max);
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class RangeAttribute : Attribute
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public RangeAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}
