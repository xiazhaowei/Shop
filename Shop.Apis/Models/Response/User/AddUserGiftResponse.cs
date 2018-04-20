using System;

namespace Shop.Api.Models.Response.User
{
    public class AddUserGiftResponse:BaseApiResponse
    {
        public Guid UserGiftId { get; set; }
    }
}