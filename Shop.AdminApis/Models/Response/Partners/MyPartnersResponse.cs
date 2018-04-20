using System.Collections.Generic;

namespace Shop.Api.Models.Response.Partners
{
    public class MyPartnersResponse:BaseApiResponse
    {
        public IList<Partner> Partners { get; set; }
    }
}