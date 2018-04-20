namespace Shop.Api.Models.Request.ThirdCurrencys
{
    public class AddThirdCurrencyRequest
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Icon { get; set; }
        public decimal Conversion { get; set; }
        public string Remark { get; set; }
        public bool IsLocked { get; set; }
    }
}