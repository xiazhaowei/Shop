using System;

namespace Shop.Domain.Events.Wallets
{
    [Serializable]
    public class IncentiveUserBenevolenceEvent:WalletEvent
    {
        public decimal BenevolenceIndex { get; private set; }
        /// <summary>
        /// 本次激励收益
        /// </summary>
        public decimal IncentiveValue { get; private set; }
        /// <summary>
        /// 善心扣除量
        /// </summary>
        public decimal BenevolenceDeduct { get; private set; }

        public IncentiveUserBenevolenceEvent() { }
        public IncentiveUserBenevolenceEvent(Guid userId,decimal benevolenceIndex,decimal incentiveValue,decimal benevolenceDeduct) :base(userId)
        {
            BenevolenceIndex = benevolenceIndex;
            IncentiveValue = incentiveValue;
            BenevolenceDeduct = benevolenceDeduct;
        }
    }
}
