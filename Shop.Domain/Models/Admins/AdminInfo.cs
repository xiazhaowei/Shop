using Shop.Common.Enums;

namespace Shop.Domain.Models.Admins
{
    public class AdminInfo
    {
        public string Name { get;private set; }
        public string LoginName { get;private set; }
        public string Portrait { get; private set; }
        public string Password { get; set; }
        public AdminRole Role { get; private set; }
        public bool IsLocked { get; private set; }

        public AdminInfo(string name,string loginName,string portrait,string password,AdminRole role,bool isLocked)
        {
            Name = name;
            LoginName = loginName;
            Portrait = portrait;
            Password = password;
            Role = role;
            IsLocked = isLocked;
        }
    }
}
