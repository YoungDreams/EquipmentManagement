using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Entities.DetailViews;

namespace PensionInsurance.Web.Views.Customer
{
    public class CustomerDetailViewModel
    {
        private readonly UrlHelper _urlHelper;

        public CustomerDetailViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            CustomerInfo = new Entities.Customer();
        }
        
        public Entities.Customer CustomerInfo { get; set; }
        public IEnumerable<ContractDetail> CustomerContracts { get; set; }
        public IEnumerable<Entities.ContractCostChange> ContractCostChanges { get; set; }
        public IEnumerable<Entities.ContractServicePackChange> ContractServicePackChanges { get; set; }
        public IEnumerable<ContractRoomChangeDetail> ContractRoomChanges { get; set; }
        public IEnumerable<Entities.CustomerLivingHistory> CustomerLivingHistories { get; set; }
        public IEnumerable<Entities.CustomerExpenseHistory> CustomerExpenseHistories { get; set; }

    }
}