using System;

namespace Shop.QueryServices.Dtos
{
    public class OperatRecord
    {
        public Guid Id { get; set; }
        public Guid AdminId { get; set; }
        public Guid AboutId { get; set; }
        public string AdminName { get; set; }
        public string Operat { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
