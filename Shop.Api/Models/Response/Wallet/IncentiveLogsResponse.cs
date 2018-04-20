using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.Wallet
{
    public class IncentiveLogsResponse : BaseApiResponse
    {
        public IList<IncentiveInfo> IncentiveLogs { get; set; }
    }

    public class IncentiveInfo
    {
        public Guid Id { get; set; }
        public string CreatedOn { get; set; }
        public decimal BenevolenceAmount { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public decimal Fee { get; set; }
    }

}