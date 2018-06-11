using Foundation.Data;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Sales.IncomingCall
{
    public class IndexViewModel
    {
        public PagedData<Entities.IncomingCall> Items { get; set; }
        public IncomingQuery Query { get; set; }
    }
}   