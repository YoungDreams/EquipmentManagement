using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PensionInsurance.Web.Views.Sales.SalesRoomChart
{
    public class FengMapDetailViewMdoel
    {
        public string[] FreeRoomFids { get; set; }
        public string[] OccupyRoomFids { get; set; }
        public string[] AllOccupyRoomFids { get; set; }
        public string MapId { get; set; }
        public int ProjectId { get; set; }
    }
}