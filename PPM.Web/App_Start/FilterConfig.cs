using System.Web.Mvc;
using Foundation.Core;

namespace PensionInsurance.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
//            filters.Add(new ExceptionLogFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }

    public class ExceptionLogFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            this.Log().Error(filterContext.Exception,filterContext.Exception.Message);
        }
    }
}
