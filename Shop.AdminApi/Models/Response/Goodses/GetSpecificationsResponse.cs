using System.Collections.Generic;

namespace Shop.Api.Models.Response.Goodses
{
    public class GetSpecificationsResponse : BaseApiResponse
    {
        public IList<Specification> Specifications { get; set; }
    }
}