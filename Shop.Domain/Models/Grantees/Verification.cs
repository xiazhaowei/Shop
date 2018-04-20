using System.Collections.Generic;

namespace Shop.Domain.Models.Grantees
{
    /// <summary>
    /// 项目验证信息
    /// </summary>
    public class Verification
    {
        /// <summary>
        /// 审核标题  患者：张三
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 证明资料
        /// </summary>
        public IList<string> Pics { get; set; }
        /// <summary>
        /// 已审核的项目  身份证已审核；诊断证明已提交；诊断医院已审核
        /// </summary>
        public IList<string> VerifiedNames { get; set; }

    }

    
}
