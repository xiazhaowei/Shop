using ENode.Commanding;
using Shop.Api.Models.Response.Partners;
using Shop.Api.Services;
using Shop.QueryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xia.Common.Extensions;

namespace Shop.Api.Controllers
{
    public class PartnerController:BaseApiController
    {
        private IPartnerQueryService _partnerQueryService;
        private IUserQueryService _userQueryService;

        public PartnerController(ICommandService commandService, IContextService contextService,
            IPartnerQueryService partnerQueryService,
            IUserQueryService userQueryService):base(commandService,contextService)
        {
            _partnerQueryService = partnerQueryService;
            _userQueryService = userQueryService;
        }

        /// <summary>
        /// 我的代理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public MyPartnersResponse MyPartners()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            var partners = _partnerQueryService.UserPartners(currentAccount.UserId.ToGuid());
            return new MyPartnersResponse
            {
                Partners = partners.Select(x => new Partner
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Mobile = x.Mobile,
                    Region = x.Region,
                    Level = x.Level.ToDescription(),
                    BalancedDate = x.BalancedDate.ToString("yyyy-MM-dd"),
                    NextBalancedDate = x.BalancedDate.AddDays(x.BalanceInterval).ToString("yyyy-MM-dd"),
                    Persent = x.Persent,
                    BalanceInterval=x.BalanceInterval,
                    CashPersent=x.CashPersent,
                    LastBalancedAmount=x.LastBalancedAmount,
                    TotalBalancedAmount=x.TotalBalancedAmount,
                    Remark=x.Remark,
                    IsLocked = x.IsLocked,
                    CreatedOn = x.CreatedOn.GetTimeSpan()
                }).ToList()
            };
        }
        
    }
}