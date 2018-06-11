using System.Web.Mvc;
using Foundation.Core;

namespace PPM.MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    public class ExceptionLogFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            this.Log().Error(filterContext.Exception, filterContext.Exception.Message);
        }
    }
}
