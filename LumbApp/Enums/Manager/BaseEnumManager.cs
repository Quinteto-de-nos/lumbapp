using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace LumbApp.Enums
{
    public class BaseEnumManager<T>
    {
        public static string GetName(T type)
        {
            return Enum.GetName(typeof(T), type);
        }

        public static byte GetValue(T type)
        {
            return Convert.ToByte(type);
        }

        public static T GetEnum(byte n)
        {
            return (T)Enum.Parse(typeof(T), n.ToString());
        }

        public static List<T> GetList()
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToList();
        }

        public static string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }
}
