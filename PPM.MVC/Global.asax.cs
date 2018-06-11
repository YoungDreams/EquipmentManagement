using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Foundation.Core;
using Foundation.Web.Mvc;
using PdfSharp.Fonts;
using PPM.Converters;
using FluentValidation.Mvc;
using PPM.CommandHandlers.Validators;

namespace PPM.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new WindsorContainer();
            Bootstrapper.Bootstrap(container);

            GlobalFontSettings.FontResolver = new CustomFontResolver();

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new WindsorValidatorFactory(container)));
        }

        protected void Application_BeginRequest(object sender, EventArgs args)
        {
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
        }

        protected void Application_Error(object sender, EventArgs args)
        {
            var error = Server.GetLastError();
            if (error != null)
            {
                this.Log().Error(error.ToString());
            }
        }
    }
}
