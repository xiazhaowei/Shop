using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    /// <summary>
    /// ��ѯ����ӿ�
    /// </summary>
    public interface ICategoryQueryService
    {
        Category Find(Guid id);

        IEnumerable<Category> RootCategorys();

        IEnumerable<Category> GetChildren(Guid id);

    }
}