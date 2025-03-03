using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using TENBookingSystem.Entities.Members;

namespace TENBookingSystem.Application.Utility
{
    public static class HelperUtility
    {      
        public static bool CheckDateFormat(string InputDate, string format)
        {

            if (!DateTime.TryParseExact(InputDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return false; // If any date doesn't match, return false
            }
            return true;
        }
        public static string GetEnumDescriptionByValue<T>(int value) where T : Enum
        {
            T enumValue = (T)Enum.ToObject(typeof(T), value);
            FieldInfo field = typeof(T).GetField(enumValue.ToString());

            DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? enumValue.ToString();
        }
    }
   
}
