using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace StudentEnrollmentSystem.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()?.GetMember(enumValue.ToString())?.First()?.GetCustomAttribute<DisplayAttribute>()?.Name;
        }

        public static List<int> GetValues(this Type enumType)
        {
            return System.Enum.GetValues(enumType).Cast<int>().Select(v => v).ToList();
        }

        public static int GetValue(this System.Enum enumValue)
        {
            return (int)(object)enumValue;
        }
    }
}