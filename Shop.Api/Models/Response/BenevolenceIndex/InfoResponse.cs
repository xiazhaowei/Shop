using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.BenevolenceIndex
{
    public class InfoResponse:BaseApiResponse
    {
        public decimal CurrentBenevolenceIndex { get; set; }
        public int StoreCount { get; set; }
        public int ConsumerCount { get; set; }
        public int PasserCount { get; set; }
        public int AmbassadorCount { get; set; }
    }
}