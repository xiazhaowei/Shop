using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    public interface IPartnerQueryService
    {
        Partner Find(Guid id);
        Partner FindByUserId(Guid userId);
        IEnumerable<Partner> Partners();
        IEnumerable<Partner> UserPartners(Guid userId);
    }
}
