using System.ComponentModel;

namespace Shop.Common.Enums
{
    /// <summary>
    /// 联盟级别
    /// </summary>
    public enum PartnerLevel
    {
        All=-1,
        /// <summary>
        /// 省级
        /// </summary>
        [Description("省级")]
        Province = 0,
        /// <summary>
        /// 市级
        /// </summary>
        [Description("市级")]
        City = 1,
        /// <summary>
        /// 县级
        /// </summary>
        [Description("县级")]
        County = 2
    }
}
