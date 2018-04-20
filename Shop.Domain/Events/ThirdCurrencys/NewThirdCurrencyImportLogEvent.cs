using ENode.Eventing;
using Shop.Domain.Models.ThirdCurrencys;
using System;

namespace Shop.Domain.Events.ThirdCurrencys
{
    [Serializable]
    public class NewThirdCurrencyImportLogEvent:DomainEvent<Guid>
    {
        public ImportLogInfo LogInfo { get; set; }

        public NewThirdCurrencyImportLogEvent() { }
        public NewThirdCurrencyImportLogEvent(ImportLogInfo logInfo)
        {
            LogInfo = logInfo;
        }
    }
}
