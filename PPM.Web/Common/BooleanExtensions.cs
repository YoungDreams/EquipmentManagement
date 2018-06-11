namespace PensionInsurance.Web.Common
{
    public static class BooleanExtensions
    {
        public static string GetText(this bool? value, string trueText, string falseText, string defaultText = null)
        {
            if (!value.HasValue)
                return defaultText;
            return value.Value ? trueText : falseText;
        }
    }
}