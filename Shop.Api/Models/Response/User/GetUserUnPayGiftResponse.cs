using System;

namespace Shop.Api.Models.Response.User
{
    public class GetUserUnPayGiftResponse:BaseApiResponse
    {
        public UserGift UserGift { get; set; }
    }

    public class UserGift
    {
        public Guid Id { get; set; }
        public string GiftName { get; set; }
        public string GiftSize{ get; set; }

        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Region { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }

    }
}