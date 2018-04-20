using System.Collections.Generic;

namespace Shop.Api.Models.Response.Statisticses
{
    public class BenevolenceIndexLogsResponse:BaseApiResponse
    {
        public IList<string> Dates { get; set; }
        public IList<decimal> BenevolenceIndexs { get; set; }
    }
}