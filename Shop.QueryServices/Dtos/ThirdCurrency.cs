using System;

namespace Shop.QueryServices.Dtos
{
    public class ThirdCurrency
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string CompanyName { get; set; }
        public decimal Conversion { get; set; }
        public decimal ImportedAmount { get; set; }
        public decimal MaxImportAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsLocked { get; set; }
        public string Remark { get; set; }
    }
}
