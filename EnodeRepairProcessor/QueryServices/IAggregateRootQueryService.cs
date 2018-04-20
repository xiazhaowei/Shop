using EnodeRepairProcessor.QueryServices.Dtos;
using System.Collections.Generic;

namespace EnodeRepairProcessor.QueryServices
{
    public interface IAggregateRootQueryService
    {
        /// <summary>
        /// Node 聚合跟
        /// </summary>
        /// <returns></returns>
        IEnumerable<AggregateRootDto> AggregateRoots();

        /// <summary>
        /// 获取 事件序列中的最新事件版本号
        /// </summary>
        /// <param name="publishedVersion"></param>
        /// <returns></returns>
        EventStreamDto EventStreamVersion(AggregateRootDto publishedVersion);
    }
}
