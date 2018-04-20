using ECommon.Components;
using ECommon.Logging;
using EnodeRepairProcessor.QueryServices;
using EnodeRepairProcessor.QueryServices.Dtos;
using System.Linq;

namespace EnodeRepairProcessor.Services
{
    [Component]
    public class RepairProcessor
    {
        private static ILogger _logger;
        private IGenerator _generator;
        private IAggregateRootQueryService _aggregateRootQueryService;

        public RepairProcessor(IGenerator generator,IAggregateRootQueryService aggregateRootQueryService)
        {
            _generator = generator;
            _aggregateRootQueryService = aggregateRootQueryService;
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(RepairProcessor).FullName);
        }

        
        /// <summary>
        /// 参照ENode 数据库修复聚合跟
        /// </summary>
        public void DoWarkReferToNodeDb()
        {
            string[] exceptAggregateRootType = { "Shop.Domain.Models.Wallets.CashTransfers.CashTransfer",
                "Shop.Domain.Models.Wallets.BenevolenceTransfers.BenevolenceTransfer",
                "Shop.Domain.Models.Wallets.ShopCashTransfers.ShopCashTransfer" };
            var aggregateRootDtos = _aggregateRootQueryService.AggregateRoots().Where(x=> !exceptAggregateRootType.Contains(x.AggregateRootTypeName));
            var total = aggregateRootDtos.Count();
            int current = 0;
            foreach (var aggregateRootDto in aggregateRootDtos)
            {
                current++;
                //事件序列中的最新序列
                var eventStreamDto = _aggregateRootQueryService.EventStreamVersion(aggregateRootDto);
                if (eventStreamDto.Version>aggregateRootDto.Version)
                {
                    //说明有未消费的事件
                    var delRecords = _generator.DelEventStream(new EventStreamDto
                    {
                        AggregateRootId = aggregateRootDto.AggregateRootId,
                        Version = aggregateRootDto.Version
                    });
                    _logger.Info(string.Format(@"聚合跟{0}，聚合跟{1}，修复完成，删除不正常事件 {2} 条", 
                        aggregateRootDto.AggregateRootTypeName, 
                        aggregateRootDto.AggregateRootId, delRecords));
                }
                if(aggregateRootDto.Version>eventStreamDto.Version)
                {

                    _generator.UpdatePublishedVersion(new AggregateRootDto
                    {
                        AggregateRootId = aggregateRootDto.AggregateRootId,
                        AggregateRootTypeName = aggregateRootDto.AggregateRootTypeName,
                        Version = eventStreamDto.Version
                    });
                    _logger.Info(string.Format(@"聚合跟{0}，聚合跟{1}，修复完成，更新最后版本为{2}", 
                        aggregateRootDto.AggregateRootTypeName,
                        aggregateRootDto.AggregateRootId, eventStreamDto.Version));
                }
                _logger.Info(string.Format(@"处理进度：{0}/{1}", current, total));
            }
        }
    }
}
