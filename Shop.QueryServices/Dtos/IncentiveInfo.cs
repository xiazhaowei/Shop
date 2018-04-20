using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.QueryServices.Dtos
{
    public class IncentiveInfo
    {
        public DateTime CreatedOn { get; set; }
        public decimal BenevolenceAmount { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public decimal Fee { get; set; }
    }
}
