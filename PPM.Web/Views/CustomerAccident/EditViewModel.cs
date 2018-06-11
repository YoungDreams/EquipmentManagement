using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.CustomerAccident
{
    public class EditViewModel : EditCustomerAccidentCommand
    {
        public IEnumerable<SelectListItem> NervousDiseaseList { get; set; }
        public IEnumerable<SelectListItem> CardiovascularDiseaseList { get; set; }
        public IEnumerable<SelectListItem> EndocrineDiseaseList { get; set; }
        public IEnumerable<SelectListItem> RespiratoryDiseaseList { get; set; }
        public IEnumerable<SelectListItem> UrinaryDiseaseList { get; set; }
        public IEnumerable<SelectListItem> DigestiveDiseaseList { get; set; }
        public IEnumerable<SelectListItem> SensoryDiseaseList { get; set; }
        public IEnumerable<SelectListItem> MovementDiseaseList { get; set; }
        public CustomerAccount CustomerAccount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSex { get; set; }
        public int CustomerAge { get; set; }
        public string ProjectName { get; set; }
        public Room Room { get; set; }
        public Bed Bed { get; set; }


    }
}