using System;

namespace Shop.Domain.Models.Stores
{
    /// <summary>
    /// 主体信息
    /// </summary>
    [Serializable]
    public class SubjectInfo
    {
        public string SubjectName { get;private set; }
        public string SubjectNumber { get;private set; }
        public string SubjectPic { get;private set; }

        public SubjectInfo(string name,string number,string pic)
        {
            SubjectName = name;
            SubjectNumber = number;
            SubjectPic = pic;
        }
    }
}
