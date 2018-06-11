using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.Purchase.OrderAcceptance
{
    public class DetailViewModel
    {
        private readonly IFetcher _fetcher;

        public DetailViewModel(IFetcher fetcher)
        {
            _fetcher = fetcher;
            PurchaseOrderItems = new List<PurchaseOrderItem>();
            PurchaseOrderOtherItems = new List<PurchaseOrderOtherItem>();
        }

        public PurchaseOrder PurchaseOrder { get; set; }
        public PurchaseOrderAcceptance PurchaseOrderAcceptance { get; set; }
        public List<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public List<PurchaseOrderOtherItem> PurchaseOrderOtherItems { get; set; }
        public int TotalProductQuantity { get; set; }
        public decimal TotalProductPrice { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public WorkflowProgress CurrentWorkflowProgress { get; set; }
        public int CurrentWorkflowStepId { get; set; }
        public int GetStock(int pruductSupplierId, int projectId)
        {
            var productStock =
                _fetcher.Query<Entities.PurchaseProductStock>()
                    .FirstOrDefault(x => x.PurchaseProductSupplier.Id == pruductSupplierId && x.Project.Id == projectId);
            return productStock?.Stock ?? 0;
        }
    }
}