using System;
using Xia.Common;

namespace Shop.Apis.ViewModels
{

    public class UserViewModel
    {

        public string NickName { get; set; }
        public string Password { get; set; }
        public string Portrait { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Region { get; set; }
        public string Role { get; set; }

        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid WalletId { get; set; }
        public Guid CartId { get; set; }
    }
}