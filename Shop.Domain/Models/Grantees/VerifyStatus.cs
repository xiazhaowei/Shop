using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Grantee
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum VerifyStatus
    {
        Verifing = 0,//审核中
        Reject = 1,//驳回
        Verifed = 2//已审核
    }
}
