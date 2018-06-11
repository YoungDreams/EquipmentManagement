namespace PensionInsurance.Web.Common
{
    public static class Numbers
    {
        public static string FormatString(this decimal value)
        {
            return value.ToString("F2");
        }

        public static string FormatString(this double value)
        {
            return value.ToString("F2");
        }
    }

    public class FormatString
    {
        public const string Decimal = "{0:0.00}";
    }
}