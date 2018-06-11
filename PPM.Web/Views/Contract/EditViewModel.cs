using PensionInsurance.Commands;
using PensionInsurance.Web.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.Contract
{
    public class EditViewModel : EditContractCommand
    {
        public Entities.Customer Customer { get; set; }
        public ProjectViewModel ProjectViewModel { get; set; }

        public IEnumerable<Entities.ContractCostChange> ContractCostChangeList { get; set; }
        public IEnumerable<Entities.ContractServicePackChange> ContractServicePackChangeList { get; set; }
        public IEnumerable<Entities.DetailViews.ContractRoomChangeDetail> ContractRoomChangeList { get; set; }

        public IEnumerable<SelectListItem> ServiceLevelList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }

        public bool IsCompartmentInfo { get; set; }
        public Entities.Building BuildingInfo { get; set; }
        public Entities.Unit UnitInfo { get; set; }
        public Entities.Floor FloorInfo { get; set; }
        public Entities.Room RoomInfo { get; set; }
        public Entities.Bed BedInfo { get; set; }
        public string CreatedBy { get; set; }
        public bool DisableCreateButton => ContractServicePackChangeList.Any(x => x.Status != ContractAddtionalStatus.生效);
        public bool DisableCreateRoomChangeButton => ContractRoomChangeList.Any(x => x.Status != ContractAddtionalStatus.生效);
        public TrackingResultViewModel TrackingResult { get; set; }

        private readonly UrlHelper _urlHelper;
        public EditViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public object DraftAndDeleteContract(int contractId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DraftAndDelete", "Contract"),
                Command = new DraftAndDeleteContractCommand {ContractId = contractId}
            };
        }
    }
}