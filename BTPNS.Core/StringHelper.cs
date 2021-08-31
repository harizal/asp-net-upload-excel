using System.Text.RegularExpressions;

namespace BTPNS.Core
{
    public static class ExtensionMethods
    {
        public static string RemoveSpecialCharacter(this string str)
        {
            if (str == null)
                return string.Empty;
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
    }
}
