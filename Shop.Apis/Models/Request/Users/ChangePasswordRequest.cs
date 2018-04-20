namespace Shop.Api.Models.Request.Users
{
    public class ChangePasswordRequest
    {
        /// <summary>
        ///  旧密码，6 到 20 个字节，不能包含空格
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        ///  新密码，6 到 20 个字节，不能包含空格
        /// </summary>
        public string NewPassword { get; set; }
    }
}