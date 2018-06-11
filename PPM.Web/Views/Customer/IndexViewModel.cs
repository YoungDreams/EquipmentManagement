using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Customer
{
    public class IndexViewModel
    {
        public CustomerQuery Query { get; set; }
        public PagedData<CustomerCheckInDetail> CustomerCheckInDetails { get; set; }
        public PagedData<CustomerCheckOutDetail> CustomerCheckOutDetails { get; set; }
        public PagedData<Entities.Customer> Customers { get; set; }
        public CustomerType? CustomerType { get; set; }
    }
}