using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.InvestmentProject
{
    public class CreateViewModel : CreateInvestmentProjectCommand
    {
        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
    }

    public class ProjectRoomBedAmount
    {
        public int RoomAmount { get; set; }
        public int BedAmount { get; set; }
    }
}