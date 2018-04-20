using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    public interface ISectionQueryService
    {
        /// <summary>
        /// 寻找所有行业
        /// </summary>
        /// <returns></returns>
        IEnumerable<SectionInfo> FindAll();
        /// <summary>
        /// 返回所有行业和统计信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<SectionAndStatistic> FindAllInculdeStatistic();
        /// <summary>
        /// 返回某个行业的 dynamic类型信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        dynamic FindDynamic(Guid id, string option);
        /// <summary>
        /// 返回某个行业和统计信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SectionAndStatistic FindInculdeStatisticById(Guid id);
    }
}
