using ENode.Domain;
using Shop.Common.Enums;
using Shop.Domain.Events.Wallets.BenevolenceTransfers;
using System;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Wallets.BenevolenceTransfers
{
    /// <summary>
    /// 善心记录 聚合跟
    /// </summary>
    public class BenevolenceTransfer:AggregateRoot<Guid>
    {
        private Guid _walletId;//钱包Id
        private string _nunber;//流水号
        private BenevolenceTransferInfo _info;//转账详情
        private BenevolenceTransferType _type;//转账类型
        private BenevolenceTransferStatus _status;//状态


        public BenevolenceTransfer(Guid id, Guid walletId, string number,BenevolenceTransferInfo info, BenevolenceTransferType type, BenevolenceTransferStatus status)
            : base(id)
        {
            id.CheckNotEmpty(nameof(id));
            walletId.CheckNotEmpty(nameof(walletId));
            info.CheckNotNull(nameof(info));
            
            ApplyEvent(new BenevolenceTransferCreatedEvent(walletId,number ,info, type,status));
        }

        public void SetStateSuccess(decimal finallyValue)
        {
            ApplyEvent(new BenevolenceTransferStatusChangedEvent(BenevolenceTransferStatus.Success,finallyValue));
        }

        public BenevolenceTransferInfo GetInfo()
        {
            return _info;
        }

        #region Handle
        private void Handle(BenevolenceTransferCreatedEvent evnt)
        {
            _id = evnt.AggregateRootId;
            _nunber = evnt.Number;
            _info = evnt.Info;
            _type = evnt.Type;
            _walletId = evnt.WalletId;
            _status = evnt.Status;
        }
        private void Handle(BenevolenceTransferStatusChangedEvent evnt)
        {
            _status = evnt.Status;
        }
        #endregion
    }


    

    
}
