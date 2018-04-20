using System.Collections.Generic;

namespace Shop.Api.Models.Response.Statisticses
{
    public class ProvinceTodaySaleResponse:BaseApiResponse
    {
        public IList<string> Provinces { get; set; }
        public IList<decimal> Sales { get; set; }
    }
}