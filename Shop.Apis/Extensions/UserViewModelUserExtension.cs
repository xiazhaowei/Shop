using Shop.Apis.ViewModels;
using Shop.Commands.Users;
using Xia.Common.Extensions;
using Xia.Common.Secutiry;
using Dtos = Shop.QueryServices.Dtos;

namespace Shop.Apis.Extensions
{
    public static class UserViewModelUserExtension
    {
        public static CreateUserCommand ToCreateUserCommand(this UserViewModel userViewModel)
        {
            var command = new CreateUserCommand(
                userViewModel.Id,
                userViewModel.ParentId,//推荐人id
                userViewModel.NickName,
                userViewModel.Portrait,
                userViewModel.Gender,
                userViewModel.Mobile,
                userViewModel.Region,
                PasswordHash.CreateHash(userViewModel.Password),
                "");
            command.AggregateRootId = userViewModel.Id;
            
            return command;
        }

        public static UserViewModel ToUserModel(this Dtos.User value)
        {
            return new UserViewModel()
            {
                Id = value.Id,
                ParentId=value.ParentId,
                WalletId=value.WalletId,
                CartId=value.CartId,
                Mobile = value.Mobile,
                Password = value.Password,
                NickName = value.NickName,
                Portrait = value.Portrait,
                Gender = value.Gender,
                Region = value.Region,
                Role=value.Role.ToDescription()
            };
        }

        
    }
}