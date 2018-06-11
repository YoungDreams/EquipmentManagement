using System.Web.Mvc;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports
{
    public class ReportsController : AuthorizedController
    {
        public ReportsController()
        {
            
        }

        public ActionResult Index()
        {
            return View("Index");
        }
    }
}