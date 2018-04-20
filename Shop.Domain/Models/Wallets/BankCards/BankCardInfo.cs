using System;

namespace Shop.Domain.Models.Wallets.BankCards
{
    [Serializable]
    public class BankCardInfo
    {
        public string BankName { get;private set; }
        public string OwnerName { get;private set; }
        public string Number { get;private set; }

        public BankCardInfo(string bankName,string ownerName,string number)
        {
            BankName = bankName;
            OwnerName = ownerName;
            Number = number;
        }
    }
}
