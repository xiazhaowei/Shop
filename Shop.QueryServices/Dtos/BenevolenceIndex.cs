using System;

namespace Shop.QueryServices.Dtos
{
    public class BenevolenceIndex
    {
        public Guid Id { get; set; }
        public decimal BIndex { get; set; }
        public decimal BenevolenceAmount { get; set; }
        public decimal IncentivedAmount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
