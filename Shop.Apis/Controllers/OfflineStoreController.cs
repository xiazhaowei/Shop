using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.OfflineStores;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.OfflineStores;
using Shop.Apis.Extensions;
using Shop.Apis.Services;
using Shop.Commands.OfflineStores;
using Shop.QueryServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common.Extensions;

namespace Shop.Apis.Controllers
{
    [Route("[controller]")]
    public class OfflineStoreController:BaseApiController
    {

        private IOfflineStoreQueryService _offlineStoreQueryService;//Q端
        private IUserQueryService _userQueryService;

        public OfflineStoreController(ICommandService commandService,IContextService contextService,
            IOfflineStoreQueryService offlineStoreQueryService,
            IUserQueryService userQueryService) : base(commandService,contextService)
        {
            _offlineStoreQueryService = offlineStoreQueryService;
            _userQueryService = userQueryService;
        }

        /// <summary>
        /// 店铺列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("OfflineStores")]
        public BaseApiResponse OfflineStores([FromBody]OfflineStoresRequest request)
        {
            request.CheckNotNull(nameof(request));
            //查询
            var offlineStores = _offlineStoreQueryService.StoreList().Where(x=>!x.IsLocked);
            var pageSize = 20;
            var total = offlineStores.Count();
            //筛选
            if (!request.Name.IsNullOrEmpty())
            {
                offlineStores = offlineStores.Where(x => x.Name.Contains(request.Name));
            }
            total = offlineStores.Count();
            //分页
            offlineStores=offlineStores.Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new OfflineStoresResponse
            {
                Total= total,
                OfflineStores= offlineStores.Select(x=>new OfflineStore {
                    Id=x.Id,
                    UserId=x.UserId,
                    Name=x.Name,
                    Thumb=x.Thumb,
                    Phone=x.Phone,
                    Description=x.Description,
                    Region=x.Region,
                    Address=x.Address,
                    Persent=x.Persent,
                    Labels=x.Labels.Split("|",true),
                    Longitude=x.Longitude,
                    Latitude=x.Latitude,
                    IsLocked=x.IsLocked,
                    CreatedOn=x.CreatedOn.GetTimeSpan()
                }).ToList()
            };
        }

        /// <summary>
        /// 我的线下店铺
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("MyOfflineStores")]
        public ListPageResponse MyOfflineStores()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

            var offlineStores = _offlineStoreQueryService.UserStoreList(currentAccount.UserId.ToGuid());
            return new ListPageResponse
            {
                Total = offlineStores.Count(),
                OfflineStores = offlineStores.Select(x => new OfflineStore
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                    Thumb = x.Thumb,
                    Phone = x.Phone,
                    Region = x.Region,
                    Address = x.Address,
                    Labels = x.Labels.Split("|", true),
                    Persent=x.Persent,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    TodaySale=x.TodaySale,
                    TotalSale=x.TotalSale,
                    IsLocked = x.IsLocked,
                    CreatedOn = x.CreatedOn.GetTimeSpan()
                }).ToList()
            };
        }

        /// <summary>
        /// 店铺信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("OfflineStore/Info")]
        public BaseApiResponse Info([FromBody]InfoRequest request)
        {
            request.CheckNotNull(nameof(request));
            var storeInfo = _offlineStoreQueryService.Info(request.Id);
            if (storeInfo == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有店铺" };
            }
            
            return new OfflineStoreInfoResponse
            {
                OfflineStore = new OfflineStore
                {
                    Id = storeInfo.Id,
                    UserId=storeInfo.UserId,
                    Name = storeInfo.Name,
                    Thumb=storeInfo.Thumb,
                    Phone=storeInfo.Phone,
                    Description = storeInfo.Description,
                    Labels=storeInfo.Labels.Split("|",true),
                    Region = storeInfo.Region,
                    Address = storeInfo.Address,
                    Persent=storeInfo.Persent,
                    Longitude=storeInfo.Longitude,
                    Latitude=storeInfo.Latitude,
                    TodaySale=storeInfo.TodaySale,
                    TotalSale=storeInfo.TotalSale,
                    IsLocked=storeInfo.IsLocked,
                    CreatedOn =storeInfo.CreatedOn.GetTimeSpan()
                }
            };
        }

        /// <summary>
        /// 店铺新的销售
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AcceptNewSale")]
        public async Task<BaseApiResponse> AcceptNewSale([FromBody]AcceptNewSaleRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

            var command = new AcceptNewSaleCommand(currentAccount.UserId.ToGuid(),request.Amount)
            {
                AggregateRootId = request.OfflineStoreId
            };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        

    }
}