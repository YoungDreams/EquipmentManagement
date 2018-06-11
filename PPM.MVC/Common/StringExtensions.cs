namespace PPM.Web.Common
{
    public static class StringExtensions
    {
        public static string SubString(this string str,int length,string apostrophe)
        {
            if (str?.Length > length)
            {
                return $"{str.Substring(0, length)}{apostrophe}";
            }
            return str;
        }
    }
}