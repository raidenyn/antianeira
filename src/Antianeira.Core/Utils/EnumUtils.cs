using System;
using System.Collections.Generic;

namespace Antianeira.Utils
{
    internal static class EnumUtils
    {
        public static Dictionary<string, int> GetValues(Type enumType)
        {
            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType);

            if (names.Length != values.Length)
            {
                throw new Exception($"Count of names is not equal count of values for enum '{enumType.Name}'");
            }

            var dic = new Dictionary<string, int>(names.Length);

            for (int i = 0; i < names.Length; i++)
            {
                dic.Add(names[i], (int)values.GetValue(i));
            }
            return dic;
        }
    }
}
