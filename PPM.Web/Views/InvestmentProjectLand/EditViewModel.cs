using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.InvestmentProjectLand
{
    public class EditViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 分期
        /// </summary>
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
        /// 建设用地面积
        /// </summary>
        public decimal BuildingLandAcreage { get; set; }
        /// <summary>
        /// 建设面积
        /// </summary>
        public decimal BuildingAcreage { get; set; }
        /// <summary>
        /// 地上建设面积
        /// </summary>
        public decimal BuildingAcreageGround { get; set; }
        /// <summary>
        /// 底下建设面积
        /// </summary>
        public decimal BuildingAcreageUnderGround { get; set; }
        /// <summary>
        /// 容积率
        /// </summary>
        public decimal Plotratio { get; set; }
        /// <summary>
        /// 所属项目
        /// </summary>
        public int InvestmentProjectId { get; set; }
    }
}