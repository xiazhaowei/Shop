using System;
using System.Collections.Generic;

namespace Shop.Domain.Models.Grantees
{
    /// <summary>
    /// 受助人基本信息
    /// </summary>
    [Serializable]
    public class GranteeInfo
    {
        public string Section { get;private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Max { get; private set; }
        public int Days { get; private set; }
        public DateTime ExpiredOn { get; private set; }
        public IList<string> Pics { get; private set; }

        public GranteeInfo(string section,
            string title,
            string description,
            decimal max,
            int days,
            DateTime expiredOn,
            IList<string>pics)
        {
            Section = section;
            Title = title;
            Description = description;
            Max = max;
            Days = days;
            ExpiredOn = expiredOn;
            Pics = pics;
        }
    }
}
