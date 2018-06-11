using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.Purchase.Order
{
    public class DetailViewModel
    {
        private readonly IFetcher _fetcher;

        public DetailViewModel(IFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        public PurchaseOrder PurchaseOrder { get; set; }
        public int TotalProductQuantity { get; set; }
        public decimal TotalProductPrice { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }
        public List<string> PurchaseSuppliers { get; set; }
        public int GetStock(int pruductSupplierId, int projectId)
        {
            var productStock =
                _fetcher.Query<Entities.PurchaseProductStock>()
                    .FirstOrDefault(x => x.PurchaseProductSupplier.Id == pruductSupplierId && x.Project.Id == projectId);
            return productStock?.Stock ?? 0;
        }
    }
}