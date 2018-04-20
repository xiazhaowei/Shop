using Shop.Apis.ViewModels;
using Dtos = Shop.QueryServices.Dtos;

namespace Shop.Apis.Extensions
{
    public static class CartViewModelCartExtension
    {
        public static CartViewModel ToCartModel(this Dtos.Cart value)
        {
            return new CartViewModel()
            {
                Id = value.Id,
                UserId=value.UserId
            };
        }
    }
}