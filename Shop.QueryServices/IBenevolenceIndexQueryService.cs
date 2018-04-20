using Shop.QueryServices.Dtos;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    public interface IBenevolenceIndexQueryService
    {
        IEnumerable<BenevolenceIndex> ListPage();
    }
}
