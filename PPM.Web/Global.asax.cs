using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Foundation.Web.Mvc;
using FluentValidation.Mvc;
using Foundation.Core;
using PdfSharp.Fonts;
using PensionInsurance.CommandHandlers.Validators;
using PensionInsurance.Converters;

namespace PensionInsurance.Web
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
