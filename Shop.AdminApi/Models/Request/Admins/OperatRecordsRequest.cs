namespace Shop.Api.Models.Request.Admins
{
    public class OperatRecordsRequest
    {
        public string AdminName { get; set; }
        public string Operat { get; set; }
        public string Remark { get; set; }
        public int Page { get; set; }
    }
}