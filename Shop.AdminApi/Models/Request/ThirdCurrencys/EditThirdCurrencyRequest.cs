using System;

namespace Shop.Api.Models.Request.ThirdCurrencys
{
    public class EditThirdCurrencyRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string CompanyName { get; set; }
        public decimal Conversion { get; set; }
        public string Remark { get; set; }
        public bool IsLocked { get; set; }
    }
}