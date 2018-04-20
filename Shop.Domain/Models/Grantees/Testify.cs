using System;

namespace Shop.Domain.Models.Grantees
{
    /// <summary>
    /// 受助人作证信息
    /// </summary>
    public class Testify
    {
        public Guid UserId { get; set; }
        public string Relationship { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string CardNumber { get; set; }
        public string Remark { get; set; }

        public Testify(Guid userId,string relationship,string name,string mobile,string cardNumber,string remark)
        {
            UserId = userId;
            Relationship = relationship;
            Name = name;
            Mobile = mobile;
            CardNumber = cardNumber;
            Remark = remark;
        }
    }
}
