using System.Collections.Generic;

namespace Shop.Api.Models.Response.OfflineStores
{
    public class ListPageResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<OfflineStore> OfflineStores { get; set; }
    }
}