using Shop.Common.Enums;

namespace Shop.Domain.Models.Admins
{
    public class AdminEditableInfo
    {
        public string Name { get;private set; }
        public string LoginName { get;private set; }
        public string Portrait { get; private set; }
        public AdminRole Role { get; private set; }
        public bool IsLocked { get; private set; }

        public AdminEditableInfo(string name,string loginName,string portrait,AdminRole role,bool isLocked)
        {
            Name = name;
            LoginName = loginName;
            Portrait = portrait;
            Role = role;
            IsLocked = isLocked;
        }
    }
}
