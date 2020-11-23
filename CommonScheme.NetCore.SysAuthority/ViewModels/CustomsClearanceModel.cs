using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonScheme.NetCore.SysAuthority.ViewModels
{
    public class PushOrder
    {
        ///<summary>
        ///入仓号
        ///</summary>
        public string InboundNo { get; set; }
        ///<summary>
        ///客户单号
        ///</summary>
        public string CustomNo { get; set; }
        ///<summary>
        ///清关号（包裹面单上派送单号）
        ///</summary>
        public string MailNo { get; set; }
        ///<summary>
        ///清关单号	
        ///</summary>
        public string BillWayNo { get; set; }
        ///<summary>
        ///出库批次号	
        ///</summary>
        public string OutBoundBatchNo { get; set; }
        ///<summary>
        ///国家代码（例：116）
        ///</summary>
        public string CountryCode { get; set; }
        ///<summary>
        ///销售平台（例：天猫出海）
        ///</summary>
        public string SellPlatform { get; set; }
        ///<summary>
        ///收件人姓名	
        ///</summary>
        public string ConsigneeName { get; set; }
        ///<summary>
        ///收件人电话	
        ///</summary>
        public string ConsigneeMobile { get; set; }
        ///<summary>
        ///收件人城市	
        ///</summary>
        public string ConsigneeCity { get; set; }
        ///<summary>
        ///收件人详细地址	
        ///</summary>
        public string ConsigneeAddress { get; set; }
        ///<summary>
        ///收件人身份证号	
        ///</summary>
        public string ConsigneeIDNo { get; set; }
        ///<summary>
        ///推送时间
        ///</summary>
        public string ActionTime { get; set; }
        ///<summary>
        ///商品集合
        ///</summary>
        public List<PushProductItem> Products { get; set; }
    }
    public class PushProductItem
    {
        ///<summary>
        ///商品中文名称
        ///</summary>
        public string CNName { get; set; }
        ///<summary>
        ///税号	
        ///</summary>
        public string TaxCode { get; set; }
        ///<summary>
        ///商品英文名称
        ///</summary>
        public string ENName { get; set; }
        ///<summary>
        ///商品规格型号（例：450mL/瓶）
        ///</summary>
        public string Spec { get; set; }
        ///<summary>
        ///商品规格型号单位（例：瓶）
        ///</summary>
        public string SpecUnit { get; set; }
        ///<summary>
        ///商品单位（例：件）
        ///</summary>
        public string Unit { get; set; }
        ///<summary>
        ///商品数量
        ///</summary>
        public int Number { get; set; }
        ///<summary>
        ///商品净重
        ///</summary>
        public decimal NetWeight { get; set; }
        ///<summary>
        ///商品毛重
        ///</summary>
        public decimal GrossWeight { get; set; }
        ///<summary>
        ///重量单位(例：kg)
        ///</summary>
        public string WeightUnit { get; set; }
        ///<summary>
        ///商品单价	
        ///</summary>
        public decimal Price { get; set; }
        ///<summary>
        ///商品总价
        ///</summary>
        public decimal TotalPrice { get; set; }
    }
    public class PushBatch
    {
        public string InboundNo { get; set; }
        public int ParcelNumber { get; set; }
        public decimal TotalWeight { get; set; }
    }
    public class ClearanceReceiptData
    {
        public string MailNo { get; set; }
        public int Action { get; set; }
        public List<string> OrderBillImage { get; set; }
        public List<string> OrderLink { get; set; }
        public string IDCardImageUrl1 { get; set; }
        public string IDCardImageUrl2 { get; set; }
        public string IDCardNo { get; set; }
        public string ActionTime { get; set; }
    }
    public class ResultHttp
    {
        public string ResponseCode { get; set; }
        public string Message { get; set; }
    }
}
