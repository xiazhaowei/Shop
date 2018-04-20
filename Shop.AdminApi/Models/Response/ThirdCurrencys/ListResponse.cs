using System.Collections.Generic;

namespace Shop.Api.Models.Response.ThirdCurrencys
{
    public class ListResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<ThirdCurrency> ThirdCurrencys { get; set; }
    }
    
}