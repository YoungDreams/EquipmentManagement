using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Sales.SalesRoomChart
{
    public class DetailViewModel
    {
        public string[] FreeRoomFids { get; set; }
        public string[] OccupyRoomFids { get; set; }
        public string[] AllOccupyRoomFids { get; set; }
        public string MapId { get; set; }
        public bool IsAreaMap { get; set; }
        public int ProjectId { get; set; }

    }
}