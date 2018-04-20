using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.BenevolenceIndexs;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 获取领域事件更新读库 基于Dapper
    /// </summary>
    public class BenevolenceIndexViewModelGenerator: BaseGenerator,
        IMessageHandler<BenevolenceIndexCreatedEvent>
    {

        /// <summary>
        /// 处理 添加
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(BenevolenceIndexCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id=evnt.AggregateRootId,
                    BIndex = evnt.BenevolenceIndex,
                    BenevolenceAmount=evnt.BenevolenceAmount,
                    IncentivedAmount=evnt.IncentivedAmount,
                    CreatedOn = evnt.Timestamp
                }, ConfigSettings.BenevolenceIndexIncentivesTable);
            });
        }
    }
}
