using ENode.Domain;
using ENode.Eventing;
using Shop.Domain.Events.ThirdCurrencys;
using System;
using System.Collections.Generic;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.ThirdCurrencys
{

    public class ThirdCurrency : AggregateRoot<Guid>
    {
        private ThirdCurrencyInfo _info;//信息
        private decimal _importedAmount;//已导入量
        private decimal _maxImportAmount;//最大可导入量
        private IList<ImportLogInfo> _importLogs;//银行卡

        public ThirdCurrency(Guid id,ThirdCurrencyInfo info):base(id)
        {
            ApplyEvent(new ThirdCurrencyCreatedEvent(info));
        }

        public void Update(ThirdCurrencyInfo info)
        {
            ApplyEvent(new ThirdCurrencyUpdatedEvent(info));
        }

        public ThirdCurrencyInfo GetInfo()
        {
            return _info;
        }

        public void Delete()
        {
            ApplyEvent(new ThirdCurrencyDeletedEvent());
        }

        /// <summary>
        /// 重置  充值数量可以为正或负
        /// </summary>
        /// <param name="amount"></param>
        public void Charge(decimal amount)
        {
            if(amount<0)
            {
                if(_maxImportAmount< Math.Abs(amount))
                {
                    throw new Exception("减少量不得大于最大可导入量");
                }
            }
            var finallyAmount = _maxImportAmount + amount;

            ApplyEvent(new ThirdCurrencyMaxImportAmountChangedEvent(finallyAmount));
        }

        /// <summary>
        /// 处理新的导入
        /// </summary>
        /// <param name="importInfo"></param>
        public void AcceptNewImport(ImportInfo importInfo)
        {
            importInfo.CheckNotNull(nameof(importInfo));

            if (_info.IsLocked)
            {
                throw new Exception("货币锁定，无法导入");
            }
            if(importInfo.Amount<=0)
            {
                throw new Exception("导入数量不正确");
            }
            //换算导入量
            var shopCashAmount = importInfo.Amount * _info.Conversion;
            //计算是否满足最后导入量
            if((_importedAmount+shopCashAmount)>_maxImportAmount)
            {
                throw new Exception("已超过最大可导入量");
            }
            var importLogInfo = new ImportLogInfo(importInfo.WalletId, 
                importInfo.Mobile, 
                importInfo.Account,
                importInfo.Amount, 
                shopCashAmount,
                _info.Conversion);

            var importedAmount = _importedAmount + shopCashAmount;

            ApplyEvent(new ThirdCurrencyImportedAmountChangedEvent(importedAmount));
            ApplyEvent(new NewThirdCurrencyImportLogEvent(importLogInfo));

        }



        #region Handle
        private void Handle(ThirdCurrencyCreatedEvent evnt)
        {
            _info = evnt.Info;
            _importedAmount = 0;
            _maxImportAmount = 0;
            _importLogs = new List<ImportLogInfo>();
        }
        private void Handle(ThirdCurrencyUpdatedEvent evnt)
        {
            _info = evnt.Info;
        }
        private void Handle(ThirdCurrencyMaxImportAmountChangedEvent evnt)
        {
            _maxImportAmount = evnt.FinallyAmount;
        }
        private void Handle(ThirdCurrencyImportedAmountChangedEvent evnt)
        {
            _importedAmount = evnt.ImportedAmount;
        }
        private void Handle(NewThirdCurrencyImportLogEvent evnt) {
            _importLogs.Add(evnt.LogInfo);
        }
        private void Handle(ThirdCurrencyDeletedEvent evnt)
        {
            _info = null;
            _importLogs = null;
        }
        #endregion
    }
}
