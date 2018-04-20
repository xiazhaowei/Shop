using ENode.Commanding;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.BenevolenceIndex;
using Shop.Api.Services;
using Shop.Common;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xia.Common.Extensions;

namespace Shop.Api.Controllers
{
    public class BenevolenceIndexController:BaseApiController
    {
        private IStoreQueryService _storeQueryService;//Q端
        private IWalletQueryService _walletQueryService;//钱包Q端
        private IBenevolenceIndexQueryService _benevolenceIndexQueryService;


        public BenevolenceIndexController(ICommandService commandService,IContextService contextService,
            IStoreQueryService storeQueryService,
            IWalletQueryService walletQueryService,
            IBenevolenceIndexQueryService benevolenceIndexQueryService
            ) : base(commandService,contextService)
        {
            _storeQueryService = storeQueryService;
            _walletQueryService = walletQueryService;
            _benevolenceIndexQueryService = benevolenceIndexQueryService;
        }
        
        /// <summary>
        /// 获取此时的善心指数和统计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse Info()
        {
            decimal currentBIndex = 0;
            //从缓存获取善心指数
            var benevolenceIndex = _apiSession.GetBenevolenceIndex();
            if (benevolenceIndex == null)
            {
                benevolenceIndex = RandomArray.BenevolenceIndex().ToString();
                _apiSession.SetBenevolenceIndex(benevolenceIndex);
            }
            currentBIndex = Convert.ToDecimal(benevolenceIndex);

            return new InfoResponse
            {
                CurrentBenevolenceIndex = currentBIndex,
                StoreCount = GenerFackCount(334, 1),
                ConsumerCount = GenerFackCount(43323, 106),
                PasserCount = GenerFackCount(2344, 8),
                AmbassadorCount = GenerFackCount(352, 2)
            };
        }

        /// <summary>
        /// 善心排行榜
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse BenevolenceRank()
        {
            int getRecordCount = 10;
            var walletAlis = _walletQueryService.BenevolenceRank(getRecordCount).ToList();
            //添加僵尸数据
            walletAlis.AddRange(FackData());
            walletAlis = walletAlis.OrderByDescending(x => x.Benevolence).ToList();

            return new BenevolenceRankResponse
            {
                WalletAlises = walletAlis.Select(x => new WalletAlis
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    NickName = x.NickName,
                    Mobile = x.Mobile,
                    Portrait=x.Portrait.ToOssStyleUrl(OssImageStyles.UserPortrait.ToDescription()),
                    Cash = x.Cash,
                    Benevolence = x.Benevolence,
                    Earnings = x.Earnings
                }).ToList()
            };
        }

        /// <summary>
        /// 指数走势
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse BenevolenceIndexLogs()
        {
            int days = -30;
            var benevolenceIndexs = _benevolenceIndexQueryService.ListPage().Where(x => x.CreatedOn > DateTime.Now.AddDays(days)).OrderBy(x => x.CreatedOn);

            var dates = benevolenceIndexs.Select(x => x.CreatedOn.ToString("MM-dd")).ToList();
            var bIndexs = benevolenceIndexs.Select(x => x.BIndex).ToList();

            return new BenevolenceIndexLogsResponse
            {
                Dates = dates,
                BenevolenceIndexs = bIndexs
            };
        }

        #region 私有方法

        /// <summary>
        /// 生成指定范围带小数的值
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        private decimal GetRandomDecimal(int min, int max, int digits = 4)
        {
            var result = new Random(Guid.NewGuid().GetHashCode()).NextDouble() * (max - min + 1) + min;
            return Math.Round(Convert.ToDecimal(result), digits);
        }


        private List<QueryServices.Dtos.WalletAlis> FackData()
        {
            var walletAlis = new List<QueryServices.Dtos.WalletAlis>();
            string[] nameArray = { "用户YUD5", "卿卿", "用户C3D5", "爱拼才会赢", "无敌", "用户YC56", "BINGGUO", "福来钱", "吃鸡666", "用户QWE2" };
            decimal[] benevolenceArray = { 20567.3461M, 19650.7641M, 18734.4261M, 16832.3461M, 9345.0087M, 9311.3461M, 7431.3891M, 6579.3207M, 5543.1123M, 4389.3461M };

            //生成10个僵尸数据
            for (int i = 0; i < Math.Min(nameArray.Length, benevolenceArray.Length); i++)
            {
                walletAlis.Add(new QueryServices.Dtos.WalletAlis
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    NickName = nameArray[i],
                    Portrait = "http://wftx-goods-img-details.oss-cn-shanghai.aliyuncs.com/default-userpic/userpic.png",
                    Cash = 10000M,
                    Benevolence = benevolenceArray[i] + GetRandomDecimal(5, 100),
                    Earnings = 10000M
                });
            }

            return walletAlis;
        }

        private int GenerFackCount(int cardinal, int step)
        {
            var oldDate = new DateTime(2017, 10, 1, 15, 36, 05);
            var days = DateTime.Now.Subtract(oldDate).Days;
            return cardinal + days * step;
        }

        #endregion
    }
}