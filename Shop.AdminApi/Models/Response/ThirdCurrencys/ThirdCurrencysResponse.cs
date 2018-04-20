using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.ThirdCurrencys
{
    public class ThirdCurrencysResponse:BaseApiResponse
    {
        public IList<ThirdCurrency> ThirdCurrencys { get; set; }
    }

    public class ThirdCurrency
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Icon { get; set; }
        public decimal Conversion { get; set; }
        public decimal ImportedAmount { get; set; }
        public decimal MaxImportAmount { get; set; }
        public string CreatedOn { get; set; }
        public string Remark { get; set; }
        public bool IsLocked { get; set; }
    }
}