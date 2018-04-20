using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    /// <summary>
    /// ��ѯ����ӿ�
    /// </summary>
    public interface IPubCategoryQueryService
    {
        PubCategory Find(Guid id);

        IEnumerable<PubCategory> RootCategorys();

        IEnumerable<PubCategory> GetChildren(Guid id);

    }
}