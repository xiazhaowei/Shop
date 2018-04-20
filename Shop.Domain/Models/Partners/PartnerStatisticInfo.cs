using System;

namespace Shop.Domain.Models.Partners
{
    /// <summary>
    /// 统计信息
    /// </summary>
    public class PartnerStatisticInfo
    {
        public decimal LastCashBalancedAmount { get; set; }
        public decimal LastBenevolenceBalancedAmount { get; set; }
        public decimal LastBalancedAmount { get; set; }//上次结算金额

        public decimal TotalCashBalancedAmount { get; set; }
        public decimal TotalBenevolenceBalancedAmount { get; set; }
        public decimal TotalBalancedAmount { get; set; }//累计结算金额

        public DateTime BalancedDate { get; set; }//已结算到日期

        public PartnerStatisticInfo(
            decimal lastCashBalancedAmount,
            decimal lastBenevolenceBalancedAmount,
            decimal lastBalancedAmount,
            decimal totalCashBalancedAmount,
            decimal totalBenevolenceBalancedAmount,
            decimal totalBalancedAmount,
            DateTime balancedDate)
        {
            LastCashBalancedAmount = lastCashBalancedAmount;
            LastBenevolenceBalancedAmount = LastBenevolenceBalancedAmount;
            LastBalancedAmount = lastBalancedAmount;

            TotalCashBalancedAmount = totalCashBalancedAmount;
            TotalBenevolenceBalancedAmount = totalBenevolenceBalancedAmount;
            TotalBalancedAmount = totalBalancedAmount;
            BalancedDate = balancedDate;
        }
    }
}
