using System;

namespace Shop.Domain.Models.Admins
{
    public class OperatRecordInfo
    {
        public string AdminName { get; set; }
        public string Operat { get; set; }
        public Guid AboutId { get; set; }
        public string Remark { get; set; }

        public OperatRecordInfo(string adminName,string operat,Guid aboutId,string remark)
        {
            AdminName = adminName;
            Operat = operat;
            AboutId = aboutId;
            Remark = remark;
        }
    }
}
