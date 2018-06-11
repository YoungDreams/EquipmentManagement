using System.Web.Mvc;
using System.Web.Routing;

namespace Foundation.Web.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string GetControllerName(this UrlHelper helper)
        {
            return helper.RequestContext.RouteData.Values["controller"].ToString();
        }

        public static string GetActionName(this UrlHelper helper)
        {
            return helper.RequestContext.RouteData.Values["action"].ToString();
        }

        public static string ReplaceCurrentUrl(this UrlHelper helper, object routeValues)
        {
            var newRouteValues = new RouteValueDictionary(routeValues);
            foreach (var key in helper.RequestContext.HttpContext.Request.QueryString.AllKeys)
            {
                if (!newRouteValues.ContainsKey(key))
                {
                    newRouteValues.Add(key, helper.RequestContext.HttpContext.Request.QueryString[key]);
                }
            }

            return helper.Action(helper.GetActionName(), helper.GetControllerName(), newRouteValues);
        }
    }
}
