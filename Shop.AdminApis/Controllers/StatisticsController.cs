using ENode.Commanding;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Statisticses;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using Xia.Common.Extensions;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
    public class StatisticsController:Controller
    {
        private IUserQueryService _userQueryService;//用户Q端
        private IWalletQueryService _walletQueryService;//钱包Q端
        private IStoreOrderQueryService _storeOrderQueryService;//订单Q端
        private IStoreQueryService _storeQueryService;//商家
        private IBenevolenceIndexQueryService _benevolenceIndexQueryService;

        public StatisticsController(ICommandService commandService,
            IUserQueryService userQueryService,
            IWalletQueryService walletQueryService,
            IStoreOrderQueryService storeOrderQueryService,
            IStoreQueryService storeQueryService,
            IBenevolenceIndexQueryService benevolenceIndexQueryService) 
        {
            _userQueryService = userQueryService;
            _walletQueryService = walletQueryService;
            _storeOrderQueryService = storeOrderQueryService;
            _storeQueryService = storeQueryService;
            _benevolenceIndexQueryService = benevolenceIndexQueryService;
        }

        /// <summary>
        /// 仪表盘
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DashBoard")]
        public BaseApiResponse DashBoard()
        {
            var users = _userQueryService.UserList();
            var wallets = _walletQueryService.AllWallets();
            var stores = _storeQueryService.StoreList();
            var storeOrders = _storeOrderQueryService.StoreOrderList();
            var rechargeApplyes = _walletQueryService.RechargeApplyLogs();
            var incentiveBenevolenceTransfers = _walletQueryService.GetBenevolenceTransfers(BenevolenceTransferType.Incentive);

            return new DashBoardResponse
            {
                UserCount = users.Count(),
                NewRegCount = users.Where(x => x.CreatedOn > DateTime.Now.AddDays(-7)).Count(),
                AmbassadorCount = users.Where(x => x.Role == UserRole.Ambassador).Count(),
                NewAmbassadorCount = users.Where(x => x.CreatedOn > DateTime.Now.AddDays(-7) && x.Role == UserRole.Ambassador).Count(),

                CashTotal = wallets.Sum(x => x.Cash),
                LockedCashTotal = wallets.Sum(x => x.LockedCash),
                RechargeApplysTotal=rechargeApplyes.Where(x=>x.Status==RechargeApplyStatus.Placed).Sum(x=>x.Amount),
                BenevolenceTotal = wallets.Sum(x => x.Benevolence),
                TodayBenevolenceAddedTotal = wallets.Sum(x => x.TodayBenevolenceAdded),
                LastIncentiveAmount= incentiveBenevolenceTransfers.Where(x=>x.CreatedOn.Date.Equals(DateTime.Now.Date)).Sum(x => x.Amount),
                TotalIncentiveAmount= incentiveBenevolenceTransfers.Sum(x=>x.Amount),

                TotalSale = stores.Sum(x => x.TotalSale),
                TodaySale = stores.Sum(x=>x.TodaySale),
                StoreOrderCount=stores.Sum(x=>x.TotalOrder),
                TodayStoreOrderCount=stores.Sum(x=>x.TodayOrder),

                TodayOrderProfit= storeOrders.Where(x=>x.CreatedOn.Date==DateTime.Now.Date).Sum(x=>x.Total)- storeOrders.Where(x => x.CreatedOn.Date == DateTime.Now.Date).Sum(x => x.StoreTotal),
                TotalOrderProfit = storeOrders.Sum(x=>x.Total)- storeOrders.Sum(x => x.StoreTotal)
            };
        }

        /// <summary>
        /// 各省市今日销售额
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ProvinceTodaySale")]
        public BaseApiResponse ProvinceTodaySale()
        {
            //今日截至到目前的订单
            var todayStoreOrders = _storeOrderQueryService.StoreOrderList();//.Where(x => x.CreatedOn.Date == DateTime.Now.Date);

            var provinces = GetProvinces();

            List<decimal> sales = new List<decimal>();
            foreach(string province in provinces)
            {
                sales.Add(todayStoreOrders.Where(x=>x.Region.Contains(province) && x.Status!=StoreOrderStatus.Placed).Sum(x=>x.Total));
            }
            return new ProvinceTodaySaleResponse
            {
                Provinces = provinces,
                Sales = sales
            };
        }

        /// <summary>
        /// 指数走势
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("BenevolenceIndexLogs")]
        public BaseApiResponse BenevolenceIndexLogs()
        {
            int days = -30;
            var benevolenceIndexs = _benevolenceIndexQueryService.ListPage().Where(x=>x.CreatedOn>DateTime.Now.AddDays(days)).OrderBy(x=>x.CreatedOn);

            var dates = benevolenceIndexs.Select(x => x.CreatedOn.ToString("MM-dd")).ToList();
            var bIndexs = benevolenceIndexs.Select(x => x.BIndex).ToList();

            return new BenevolenceIndexLogsResponse
            {
                Dates = dates,
                BenevolenceIndexs = bIndexs
            };
        }


        #region 私有方法

        private IList<string> GetProvinces()
        {
            List<string> provinces = new List<string> {
                "北京", "天津","河北","山西","内蒙古","辽宁","吉林","黑龙江","上海","江苏","浙江","安徽","福建","江西","山东","河南",
                "湖北","湖南","广东","广西","海南","重庆","四川","贵州","云南","西藏","陕西","甘肃","青海","宁夏","新疆","台湾","香港","澳门"
            };
            return provinces;
        }
        #endregion
       
    }
}