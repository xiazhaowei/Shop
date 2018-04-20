using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.Wallet
{
    public class AcceptTransferRequest
    {
        /// <summary>
        /// 收款人ID
        /// </summary>
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
    }
}