using System;
using System.Collections.Generic;
using System.Linq;

namespace SIV.Presentation.Desktop.Common
{
    internal static class QueryBuilder
    {
        public static string Build(IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return string.Join("&", parameters
                .Where(p => p.Value != null)
                .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(FormatValue(p.Value))}"));
        }

        private static string FormatValue(object value)
        {
            if (value is Enum)
                return ((int)value).ToString();
            if (value is DateTimeOffset dateTimeOffset)
                return dateTimeOffset.ToString("o");
            if (value is DateTime dateTime)
                return dateTime.ToString("s");
            return value.ToString();
        }
    }
}
