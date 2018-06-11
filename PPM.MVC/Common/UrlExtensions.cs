using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace PPM.Web.Common
{
    public static class UrlExtensions
    {
        //public static string ReplaceCurrentUrl(this UrlHelper urlHelper, object routeValues)
        //{
        //    var mergedRouteValues = new RouteValueDictionary(routeValues);
        //    foreach (var currentRouteValue in urlHelper.RequestContext.RouteData.Values)
        //    {
        //        if (!mergedRouteValues.ContainsKey(currentRouteValue.Key))
        //        {
        //            mergedRouteValues[currentRouteValue.Key] = currentRouteValue.Value;
        //        }
        //    }

        //    return urlHelper.RequestContext.RouteData.Route.GetVirtualPath(urlHelper.RequestContext, mergedRouteValues).VirtualPath;
        //}

        public static string GetControllerName(this WebViewPage page)
        {
            return page.ViewContext.RouteData.Values["controller"].ToString();
        }

        public static string GetActionName(this WebViewPage page)
        {
            return page.ViewContext.RouteData.Values["action"].ToString();
        }

        public static bool IsController(this WebViewPage page, string controller)
        {
            return page.GetControllerName().Equals(controller, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsControllerIn(this WebViewPage page, params string[] controllers)
        {
            return controllers.Any(page.IsController);
        }

        public static bool IsAction(this WebViewPage page, string action)
        {
            return page.GetActionName().Equals(action, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsAction(this WebViewPage page, string action,string controller)
        {
            return page.IsController(controller) && page.IsAction(action);
        }
    }

    
}