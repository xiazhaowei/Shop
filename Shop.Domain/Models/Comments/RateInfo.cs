using System;

namespace Shop.Domain.Models.Comments
{
    [Serializable]
    public class RateInfo
    {
        public Single Rate { get; private set; }
        public Single PriceRate { get; private set; }
        public Single DescribeRate { get; private set; }
        public Single QualityRate { get; private set; }
        public Single ExpressRate { get; private set; }

        public RateInfo(Single priceRate, Single describeRate, Single qualityRate, Single expressRate)
        {
            PriceRate = priceRate;
            DescribeRate = describeRate;
            QualityRate = qualityRate;
            ExpressRate = expressRate;
            Rate = Ave(PriceRate, DescribeRate, QualityRate, expressRate);
        }

        private Single Ave(Single target,Single target1,Single target2,Single target3)
        {
            return (target + target1 + target2 + target3) / 4;
        }
    }
}
