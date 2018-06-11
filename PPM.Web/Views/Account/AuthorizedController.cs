using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Web.Mvc;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Account
{
    public abstract class 
        MvcController : Controller
    {
        public new IVMPrincipal User { get { return base.User as VMPrincipal; } }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sw = Stopwatch.StartNew();

            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() &&
                filterContext.RequestContext.HttpContext.Request.HttpMethod.Equals("POST",
                    StringComparison.CurrentCultureIgnoreCase))
            {
                if (!ModelState.IsValid)
                {
                    filterContext.Result = Json(
                            new
                            {
                                success = false,
                                errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)
                            });
                }
            }

            this.Log().Warn($"OnActionExecuting:{sw.Elapsed.ToString()}|{filterContext.RequestContext.HttpContext.Request.Url}");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception as ApplicationException;
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() &&
               filterContext.RequestContext.HttpContext.Request.HttpMethod.Equals("POST", StringComparison.CurrentCultureIgnoreCase)
               && exception != null)
            {
                filterContext.Result = Json(new
                {
                    success = false,
                    errors = new string[] { exception.Message }
                });
                filterContext.RequestContext.HttpContext.Response.StatusCode = 200;
                filterContext.ExceptionHandled = true;

                this.Log().Error(exception.ToString());
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() &&
                filterContext.RequestContext.HttpContext.Request.HttpMethod.Equals("POST",
                    StringComparison.CurrentCultureIgnoreCase))
            {
                if (filterContext.Result is RedirectToRouteResult)
                {
                    var result = filterContext.Result as RedirectToRouteResult;
                    filterContext.Result = Json(new { success = true, redirect = Url.RouteUrl(result.RouteName, result.RouteValues) });
                }
                else if (filterContext.Result is RedirectResult)
                {
                    var result = filterContext.Result as RedirectResult;
                    filterContext.Result = Json(new { success = true, redirect = result.Url });
                }
                else if(filterContext.Result is JsonResult)
                {
                    // Do nothing
                }
                else
                {
                    filterContext.Result = Json(new { success = true });
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }

    [WebAuthorize]
    public abstract class AuthorizedController : MvcController
    {

    }
}