using ENode.Domain;
using Shop.Common.Enums;
using Shop.Domain.Events.Wallets.ShopCashTransfers;
using System;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Wallets.ShopCashTransfers
{
    /// <summary>
    /// 现金记录 聚合跟
    /// </summary>
    public class ShopCashTransfer:AggregateRoot<Guid>
    {
        private Guid _walletId;//钱包Id
        private string _nunber;//流水号
        private ShopCashTransferInfo _info;//转账详情
        private ShopCashTransferType _type;//现金转账类型
        private ShopCashTransferStatus _status;//状态


        public ShopCashTransfer(Guid id, Guid walletId, string number,ShopCashTransferInfo info, ShopCashTransferType type,ShopCashTransferStatus status)
            : base(id)
        {
            id.CheckNotEmpty(nameof(id));
            walletId.CheckNotEmpty(nameof(walletId));
            info.CheckNotNull(nameof(info));
            
            ApplyEvent(new ShopCashTransferCreatedEvent(walletId,number ,info, type,status));
        }

        public void SetStateSuccess(decimal finallyValue)
        {
            ApplyEvent(new ShopCashTransferStatusChangedEvent(ShopCashTransferStatus.Success,finallyValue));
        }

        #region 取值

        
        public ShopCashTransferInfo GetInfo()
        {
            return _info;
        }

        public ShopCashTransferType GetTransferType()
        {
            return _type;
        }

        public ShopCashTransferStatus GetTransferStatus()
        {
            return _status;
        }
        #endregion

        #region Handle
        private void Handle(ShopCashTransferCreatedEvent evnt)
        {
            _id = evnt.AggregateRootId;
            _nunber = evnt.Number;
            _info = evnt.Info;
            _type = evnt.Type;
            _status = evnt.Status;
            _walletId = evnt.WalletId;
        }
        private void Handle(ShopCashTransferStatusChangedEvent evnt)
        {
            _status = evnt.Status;
        }
        #endregion
    }


    

    
}
