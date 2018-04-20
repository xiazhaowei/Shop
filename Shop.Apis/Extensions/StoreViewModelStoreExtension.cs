using Shop.Apis.ViewModels;
using Dtos = Shop.QueryServices.Dtos;

namespace Shop.Apis.Extensions
{
    public static class StoreViewModelStoreExtension
    {
        public static StoreViewModel ToStoreModel(this Dtos.Store value)
        {
            return new StoreViewModel()
            {
                Id=value.Id,
                UserId=value.UserId,
                AccessCode=value.AccessCode,
                Name=value.Name,
                Description=value.Description,
                Region=value.Region,
                Address=value.Address
            };
        }

        
    }
}