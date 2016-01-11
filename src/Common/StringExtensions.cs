using System;

namespace GoldBoxExplorer.Lib
{
    public static class StringExtensions
    {
         public static string RemoveNulls(this string value)
         {
             var nullIndex = value.IndexOf(Convert.ToChar(0x0));
             if (nullIndex != -1)
             {
                 value = value.Substring(0, nullIndex);
             }
             return value;
         }
    }
}