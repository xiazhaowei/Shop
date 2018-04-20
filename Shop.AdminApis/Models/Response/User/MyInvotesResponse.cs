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
        public decimal TotalStoreOrderAmount { get; set; }
        public int TotalUserCount { get; set; }
        public string ParentMobile { get; set; }
    }
}