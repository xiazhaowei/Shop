using System;

namespace Shop.Domain.Models.Goodses
{
    /// <summary>
    /// 商品评价统计信息
    /// </summary>
    public class CommentStatisticInfo
    {
        /// <summary>
        /// 综合得分
        /// </summary>
        public Single Rate { get; private set; }
        /// <summary>
        /// 价格得分
        /// </summary>
        public Single PriceRate { get;private set; }
        /// <summary>
        /// 描述得分
        /// </summary>
        public Single DescribeRate { get;private set; }
        /// <summary>
        /// 品质得分
        /// </summary>
        public Single QualityRate { get;private set; }
        /// <summary>
        /// 物流得分
        /// </summary>
        public Single ExpressRate { get; private set; }
        /// <summary>
        /// 评价数量
        /// </summary>
        public int RateCount { get; private set; }

        public CommentStatisticInfo(Single rate,Single priceRate,Single describeRate,Single qualityRate,Single expressRate,int rateCount)
        {
            Rate = rate;
            PriceRate = priceRate;
            DescribeRate = describeRate;
            QualityRate = qualityRate;
            ExpressRate = expressRate;
            RateCount = rateCount;
        }

    }
}
