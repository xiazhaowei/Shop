using System;

namespace Shop.Api.Models.Response.Store
{
    public class ReturnAddressInfoResponse:BaseApiResponse
    {
        public ReturnAddressInfo ReturnAddressInfo { get; set; }
    }

    public class ReturnAddressInfo
    {
        public Guid StoreId { get; set; }
        public string ReturnAddressName { get; set; }
        public string ReturnAddressMobile { get; set; }
        public string ReturnAddress { get; set; }
    }
}