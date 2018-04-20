﻿using ENode.Commanding;
using Shop.AdminApi.Extensions;
using Shop.AdminApi.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.OfflineStores;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.OfflineStores;
using Shop.Commands.OfflineStores;
using Shop.QueryServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.AdminApi.Controllers
{
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
        

        #region 总后台管理
        /// <summary>
        /// 创建店铺
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Create(CreateOfflineStoreRequest request)
        {
            request.CheckNotNull(nameof(request));

            var user = _userQueryService.FindUser(request.Mobile);
            if (user == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有找到用户" };
            }
            var newofflinestoreid = GuidUtil.NewSequentialId();
            var command = new CreateOfflineStoreCommand(
                newofflinestoreid,
                user.Id,
                request.Name,
                request.Thumb,
                request.Phone,
                request.Description,
                request.Labels,
                request.Region,
                request.Address,
                request.Persent,
                request.Longitude,
                request.Latitude,
                request.IsLocked);

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "添加线下店铺", newofflinestoreid, request.Name);
            return new BaseApiResponse();
        }
        /// <summary>
        /// 店铺目前销售额
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public BaseApiResponse TotalTodaySale()
        {
            var todaySale = _offlineStoreQueryService.TodaySale();
            return new TotalTodaySaleResponse
            {
                TotalTodaySale = todaySale
            };
        }
        
        /// <summary>
        /// 店铺列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ListPageResponse ListPage(ListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var pageSize = 20;
            var stores = _offlineStoreQueryService.StoreList();
            var total = stores.Count();
            //筛选
            if (!request.Name.IsNullOrEmpty())
            {
                stores = stores.Where(x => x.Name.Contains(request.Name));
            }
            if (!request.Region.IsNullOrEmpty())
            {
                stores = stores.Where(x => x.Region.Contains(request.Region));
            }
            total = stores.Count();
            //分页
            stores = stores.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);
            
            return new ListPageResponse
            {
                Total = total,
                OfflineStores = stores.Select(x => new OfflineStore
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                    Thumb=x.Thumb,
                    Phone=x.Phone,
                    Region = x.Region,
                    Address = x.Address,
                    TodaySale=x.TodaySale,
                    Description=x.Description,
                    Labels=x.Labels.Split("|",true),
                    TotalSale=x.TotalSale,
                    Persent=x.Persent,
                    Longitude = x.Longitude,
                    Latitude = x.Latitude,
                    CreatedOn=x.CreatedOn.GetTimeSpan(),
                    IsLocked = x.IsLocked
                }).ToList()
            };
        }

        

        /// <summary>
        /// 修改店铺信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Edit(EditRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new UpdateOfflineStoreCommand(
                request.Name,
                request.Thumb,
                request.Phone,
                request.Description,
                request.Labels,
                request.Region,
                request.Address,
                request.Persent,
                request.Longitude,
                request.Latitude,
                request.IsLocked)
            {
                AggregateRootId = request.Id
            };

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "编辑线下店铺", request.Id, request.Name);

            return new BaseApiResponse();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Delete(DeleteRequest  request)
        {
            request.CheckNotNull(nameof(request));
            //判断
            var offlineStore = _offlineStoreQueryService.Info(request.Id);
            if (offlineStore == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该店铺" };
            }
            //删除
            var command = new DeleteOfflineStoreCommand(request.Id);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "删除线下店铺", request.Id, offlineStore.Name);

            return new BaseApiResponse();
        }

        #endregion

        

    }
}