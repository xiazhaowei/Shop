namespace Shop.Domain.Models.Grantees
{
    /// <summary>
    /// 受助情况统计信息
    /// </summary>
    public class GranteeStatisticsInfo
    {
        public decimal Total { get;  private set; }
        public decimal Goods { get;private set; }
        public int Count { get;private set; }

        public GranteeStatisticsInfo(decimal total,decimal goods,int count)
        {
            Total = total;
            Goods = goods;
            Count = count;
        }
    }
}
