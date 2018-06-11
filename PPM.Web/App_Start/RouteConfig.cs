using System.Web.Mvc;
using System.Web.Routing;

namespace PensionInsurance.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "LogOn", id = UrlParameter.Optional }
            );

            routes.MapRoute("Payments.UnionPay.POS.PaymentOrder", "Payments/UnionPay/POS/PaymentOrder",
                new {controller = "POS", action = "PaymentOrder" },new[] { "PensionInsurance.Web.Views.Payments.UnionPay.POS" });
            routes.MapRoute("Payments.UnionPay.POS.ReceiveNotification", "Payments/UnionPay/POS/ReceiveNotification",
                new { controller = "POS", action = "ReceiveNotification" }, new[] { "PensionInsurance.Web.Views.Payments.UnionPay.POS" });
        }
    }
}
