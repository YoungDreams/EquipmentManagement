using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PensionInsurance.Web.Views.Sales.OperatorConsulting
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        /// <summary>
        /// 咨询人名称
        /// </summary>
        public string VisitorName { get; set; }
        /// <summary>
        /// 咨询人电话
        /// </summary>
        public string VisitorPhone { get; set; }
        /// <summary>
        /// 咨询人类型
        /// </summary>
        public string VisitorType { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string ConsultingNo { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string ConsultingName { get; set; }
        /// <summary>
        /// 客户联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 客户年龄
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 客户性别
        /// </summary>
        public string Sex { get; set; }


        /// <summary>
        /// 项目
        /// </summary>
        public int? ProjectId { get; set; }
        /// <summary>
        /// 客户级别
        /// </summary>
        public ConsultingLevel Level { get; set; }

        /// <summary>
        /// 意向户型
        /// </summary>
        public string HouseType { get; set; }


        /// <summary>
        /// 与客户关系
        /// </summary>
        public string Relationship { get; set; }
        /// <summary>
        /// 未转交原因
        /// </summary>
        public string Nottransferred { get; set; }
        /// <summary>
        /// 健康状况
        /// </summary>
        public string HealthStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        public string VisitorSex { get; set; }

        /// <summary>
        /// 获知渠道
        /// </summary>
        public string Source { get; set; }
        public int ProjectSaleId{ get; set; }

        public DateTime? AnswerTheCall { get; set; }
        public int? CurrentConsultingProjectId { get; set; }

        public OperatorConsultingQuery Query { get; set; }
        public PagedData<OperatorConsultingDetail> Items { get; set; }
        public List<User> Sales { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }    
    }
}