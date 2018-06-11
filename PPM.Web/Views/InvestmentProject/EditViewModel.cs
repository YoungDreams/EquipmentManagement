using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.InvestmentProject
{
    public class EditViewModel : EditInvestmentProjectCommand
    {
        private readonly UrlHelper _urlHelper;

        public EditViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
        public IEnumerable<Entities.InvestmentProjectLand> InvestmentProjectLands { get; set; }
        public string Period { get; set; }
        /// <summary>
        /// 地块名称
        /// </summary>
        public string LandName { get; set; }
        /// <summary>
        /// 用地性质
        /// </summary>
        public InvestmentProjectLandCharacteristics LandCharacteristics { get; set; }
        /// <summary>
        /// 容积率
        /// </summary>
        public decimal Plotratio { get; set; }
        

        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "InvestmentProjectLand"),
                Command = new DeleteEntityCommand { EntityId = id, ReturnUrl = strUrl }
            };
        }
    }
}