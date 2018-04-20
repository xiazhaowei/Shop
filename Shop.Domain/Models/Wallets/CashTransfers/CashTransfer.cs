using ENode.Domain;
using Shop.Common.Enums;
using Shop.Domain.Events.Wallets.CashTransfers;
using System;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Wallets.CashTransfers
{
    /// <summary>
    /// 现金记录 聚合跟
    /// </summary>
    public class CashTransfer:AggregateRoot<Guid>
    {
        private Guid _walletId;//钱包Id
        private string _nunber;//流水号
        private CashTransferInfo _info;//转账详情
        private CashTransferType _type;//现金转账类型
        private CashTransferStatus _status;//状态


        public CashTransfer(Guid id, Guid walletId, string number,CashTransferInfo info, CashTransferType type,CashTransferStatus status)
            : base(id)
        {
            id.CheckNotEmpty(nameof(id));
            walletId.CheckNotEmpty(nameof(walletId));
            info.CheckNotNull(nameof(info));
            
            ApplyEvent(new CashTransferCreatedEvent(walletId,number ,info, type,status));
        }

        public void SetStateSuccess(decimal finallyValue)
        {
            ApplyEvent(new CashTransferStatusChangedEvent(CashTransferStatus.Success,finallyValue));
        }

        #region 取值

        
        public CashTransferInfo GetInfo()
        {
            return _info;
        }

        public CashTransferType GetTransferType()
        {
            return _type;
        }

        public CashTransferStatus GetTransferStatus()
        {
            return _status;
        }
        #endregion

        #region Handle
        private void Handle(CashTransferCreatedEvent evnt)
        {
            _id = evnt.AggregateRootId;
            _nunber = evnt.Number;
            _info = evnt.Info;
            _type = evnt.Type;
            _status = evnt.Status;
            _walletId = evnt.WalletId;
        }
        private void Handle(CashTransferStatusChangedEvent evnt)
        {
            _status = evnt.Status;
        }
        #endregion
    }


    

    
}
