using PensionInsurance.Entities;
using PensionInsurance.Entities.CustomerHealthItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.HealthManagement.Graphic
{
    public class GraphicViewModel
    {
        private readonly UrlHelper _url;

        public GraphicViewModel(UrlHelper url)
        {

            _url = url;
        }
        public List<HealthManageECG> Ecgs { get; set; }

        public List<HealthManageBoneDensity> BoneDensitys { get; set; }

        public int EId { get; set; }

        public int BId { get; set; }

        public string ECheckDate { get; set; }

        public string ECreateDate { get; set; }

        public string EImageData { get; set; }

        public CustomerHealthEnum EComFrom { get; set; }

        public string BdCheckDate { get; set; }

        public string BdCreateDate { get; set; }

        public string BdImageData { get; set; }

        public CustomerHealthEnum BdComFrom { get; set; }

        public int CustomerAccountId { get; set; }


        public ContractStatus Status { get; set; }


        public HealthMonitoringViewModel hmviewModel { get; set; }
    }
}