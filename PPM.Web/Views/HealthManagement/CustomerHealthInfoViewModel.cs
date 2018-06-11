using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PensionInsurance.Web.Views.HealthManagement
{
    public class CustomerHealthInfoViewModel
    {
        public int Id { get; set; }
        public string CheckDate { get; set; }
        public int Sex { get; set; }
        public string Breath { get; set; }
        public int BreathCount { get; set; }
        public bool BreathOk { get; set; }

        public string Temperature { get; set; }
        public int TemperatureCount { get; set; }
        public bool TemperatureOk { get; set; }
        public string Pulse { get; set; }
        public int PulseCount { get; set; }
        public bool PulseOk { get; set; }
        public string HeartRate { get; set; }
        public int HeartRateCount { get; set; }
        public bool HeartRateOk { get; set; }
        public string BloodPresure { get; set; }
        public int BloodPresureCount { get; set; }
        public bool BloodPresureOk { get; set; }

        public string LeftVision { get; set; }
        public int LeftVisionCount { get; set; }
        public bool LeftVisionOk { get; set; }
        public string RightVision { get; set; }
        public int RightVisionCount { get; set; }
        public bool RightVisionOk { get; set; }

        /// <summary>
        /// 总胆固醇
        /// </summary>
        public string TChol { get; set; }
        public int TCholCount { get; set; }
        public bool TCholOk { get; set; }
        public string Trig { get; set; }
        public int TrigCount { get; set; }
        public bool TrigOk { get; set; }
        public string HdlChol { get; set; }
        public int HdlCholCount { get; set; }
        public bool HdlCholOk { get; set; }
        public string CalcLdl { get; set; }
        public int CalcLdlCount { get; set; }
        public bool CalcLdlOk { get; set; }

        /// <summary>
        /// 尿胆原
        /// </summary>
        public string URO { get; set; }
        public int UROCount { get; set; }
        public bool UROOk { get; set; }
        /// <summary>
        /// 潜血
        /// </summary>
        public string BLD { get; set; }
        public int BLDCount { get; set; }
        public bool BLDOk { get; set; }
        /// <summary>
        /// 胆红表
        /// </summary>
        public string BIL { get; set; }
        public int BILCount { get; set; }
        public bool BILOk { get; set; }
        /// <summary>
        /// 酮体
        /// </summary>
        public string KET { get; set; }
        public int KETCount { get; set; }
        public bool KETOk { get; set; }
        /// <summary>
        /// 葡萄糖
        /// </summary>
        public string GLU { get; set; }
        public int GLUCount { get; set; }
        public bool GLUOk { get; set; }
        /// <summary>
        /// 蛋白质
        /// </summary>
        public string PRO { get; set; }
        public int PROCount { get; set; }
        public bool PROOk { get; set; }
        /// <summary>
        /// 酸碱度
        /// </summary>
        public string PH { get; set; }
        public int PHCount { get; set; }
        public bool PHOk { get; set; }
        /// <summary>
        /// 亚硝酸盐
        /// </summary>
        public string NIT { get; set; }
        public int NITCount { get; set; }
        public bool NITOk { get; set; }
        /// <summary>
        /// 白细胞
        /// </summary>
        public string LEU { get; set; }
        public int LEUCount { get; set; }
        public bool LEUOk { get; set; }
        /// <summary>
        /// 比重
        /// </summary>
        public string SG { get; set; }
        public int SGCount { get; set; }
        public bool SGOk { get; set; }
        /// <summary>
        /// 维生素
        /// </summary>
        public string VC { get; set; }
        public int VCCount { get; set; }
        public bool VCOk { get; set; }
        /// <summary>
        /// 隐血
        /// </summary>
        public string BLO { get; set; }
        public int BLOCount { get; set; }
        public bool BLOOk { get; set; }
        /// <summary>
        /// 微白蛋白
        /// </summary>
        public string MAL { get; set; }
        public int MALCount { get; set; }
        public bool MALOk { get; set; }
        /// <summary>
        /// 肌酐
        /// </summary>
        public string CR { get; set; }
        public int CRCount { get; set; }
        public bool CROk { get; set; }
        /// <summary>
        /// 钙离子
        /// </summary>
        public string UCA { get; set; }
        public int UCACount { get; set; }
        public bool UCAOk { get; set; }

        public string BloodSugar { get; set; }
        public int BloodSugarCount { get; set; }
        public bool BloodSugarOk { get; set; }
        public string BloodSugarType { get; set; }
        /// <summary>
        /// 血氧饱和度
        /// </summary>
        public string BloodOxygenSaturation { get; set; }
        public int BloodOxygenSaturationCount { get; set; }
        public bool BloodOxygenSaturationOk { get; set; }
        public string BloodUa { get; set; }
        public int BloodUaCount { get; set; }
        public bool BloodUaOk { get; set; }
        /// <summary>
        /// 血红蛋白
        /// </summary>
        public string Hb { get; set; }
        public int HbCount { get; set; }
        public bool HbOk { get; set; }
        /// <summary>
        /// 红细胞比容
        /// </summary>
        public string Hct { get; set; }
        public int HctCount { get; set; }
        public bool HctOk { get; set; }
        /// <summary>
        /// 糖化血红蛋白
        /// </summary>
        public string SugarHct { get; set; }
        public int SugarHctCount { get; set; }
        public bool SugarHctOk { get; set; }

        public string Height { get; set; }
        public int HeightCount { get; set; }
        public bool HeightOk { get; set; }
        public string Weight { get; set; }
        public int WeightCount { get; set; }
        public bool WeightOk { get; set; }
        public string BMI { get; set; }
        public int BMICount { get; set; }
        public bool BMIOk { get; set; }
        public string Waistline { get; set; }
        public int WaistlineCount { get; set; }
        public bool WaistlineOk { get; set; }
        public string Hipline { get; set; }
        public int HiplineCount { get; set; }
        public bool HiplineOk { get; set; }
        public string Whr { get; set; }
        public int WhrCount { get; set; }
        public bool WhrOk { get; set; }
        /// <summary>
        /// 用力肺活量
        /// </summary>
        public string VitalCapacity { get; set; }
        public int VitalCapacityCount { get; set; }
        public bool VitalCapacityOk { get; set; }
        public string MaxBreath { get; set; }
        public int MaxBreathCount { get; set; }
        public bool MaxBreathOk { get; set; }
        public string FirstSecondBreath { get; set; }
        public int FirstSecondBreathCount { get; set; }
        public bool FirstSecondBreathOk { get; set; }
        /// <summary>
        /// 肺活量
        /// </summary>
        public string VitalCapacity1 { get; set; }
        public int VitalCapacity1Count { get; set; }
        public bool VitalCapacity1Ok { get; set; }
        public string Fat { get; set; }
        public int FatCount { get; set; }
        public bool FatOk { get; set; }
        public string Water { get; set; }
        public int WaterCount { get; set; }
        public bool WaterOk { get; set; }
        /// <summary>
        /// 代谢
        /// </summary>
        public string Metabolize { get; set; }
        public int MetabolizeCount { get; set; }
        public bool MetabolizeOk { get; set; }
        public string ChineseMedical { get; set; }
        public bool HasMore { get; set; }
        //中医体质
        public string OtherPhysique { get; set; }
        public int OtherPhysiqueCount { get; set; }
        public bool OtherPhysiqueOK { get; set; }
    }

    public class HealthManageInfoViewModel {
        /// <summary>
        /// 行数
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 指标参数值
        /// </summary>
        public string ColVal { get; set; }
        /// <summary>
        /// 指标参数名
        /// </summary>
        public string ColName { get; set; }
        /// <summary>
        /// 项名
        /// </summary>
        public string TitleName { get; set; }
        /// <summary>
        /// 指标项是否异常
        /// </summary>
        public bool ColOK { get; set; }
        /// <summary>
        /// 是否可以点击
        /// </summary>
        public bool ColPoint { get; set; }
        /// <summary>
        /// 参考标准
        /// </summary>
        public string HealthStandard { get; set; }
    }
}