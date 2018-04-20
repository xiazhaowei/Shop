using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.User
{
    public class MyInvotesResponse:BaseApiResponse
    {
        public IList<dynamic> MyInvotes { get; set; }
    }
    public class UserInvotesResponse : BaseApiResponse
    {
        public IList<dynamic> Invotes { get; set; }
        public string ParentMobile { get; set; }
        public InvoteStatisticsInfo InvoteStatisticsInfo { get; set; }
    }

    public class InvoteStatisticsInfo
    {
        public int TotalUserCount { get; set; }
        public int DirectPasserCount { get; set; }
        public int TotalPasserCount { get; set; }
        public int DirectVipPasserCount { get; set; }
        public int TotalVipPasserCount { get; set; }
        public decimal TotalStoreOrderAmount { get; set; }
    }
}