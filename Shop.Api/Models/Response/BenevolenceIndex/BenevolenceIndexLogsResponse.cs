using System.Collections.Generic;

namespace Shop.Api.Models.Response.BenevolenceIndex
{
    public class BenevolenceIndexLogsResponse:BaseApiResponse
    {
        public IList<string> Dates { get; set; }
        public IList<decimal> BenevolenceIndexs { get; set; }
    }
}