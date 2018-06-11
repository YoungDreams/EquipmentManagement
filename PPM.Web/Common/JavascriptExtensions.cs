using Newtonsoft.Json;

namespace PensionInsurance.Web.Common
{
    public static class JavascriptExtensions
    {
        public static string ToJavascript(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}