using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.User
{
    public class ExpressAddressesResponse:BaseApiResponse
    {
        public IList<ExpressAddress> ExpressAddresses { get; set; }
    }

    public class ExpressAddress
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
    }
}