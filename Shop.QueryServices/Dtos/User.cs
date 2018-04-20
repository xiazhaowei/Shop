using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    /// <summary>
    /// 用户 DTO
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid WalletId { get; set; }
        public Guid CartId { get; set; }
        public string NickName { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string Portrait { get; set; }
        public string Gender { get; set; }
        public string Region { get; set; }
        public UserLock IsLocked { get; set; }
        public Freeze IsFreeze { get; set; }
        public DateTime CreatedOn { get; set; }
        public UserRole Role { get; set; }
        public string WeixinId { get; set; }
        public string UnionId { get; set; }
    }
    

}
