using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Admins
{
    public class OperatRecordsResponse : BaseApiResponse
    {
        public int Total { get; set; }
        public IList<OperatRecord> OperatRecords { get; set; }
    }

    public class OperatRecord
    {
        public Guid Id { get; set; }
        public Guid AdminId { get; set; }
        public Guid AboutId { get; set; }
        public string AdminName { get; set; }
        public string Operat { get; set; }
        public string Remark { get; set; }
        public string CreatedOn { get; set; }
    }
}