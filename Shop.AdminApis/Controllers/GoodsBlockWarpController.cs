using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.GoodsBlockWarps;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.GoodsBlocks;
using Shop.Commands.GoodsBlocks;
using Shop.QueryServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
    public class GoodsBlockWarpController : BaseApiController
    {
        private IGoodsBlockWarpQueryService _goodsBlockWarpQueryService;

        public GoodsBlockWarpController(ICommandService commandService,IContextService contextService,
            IGoodsBlockWarpQueryService goodsBlockWarpQueryService) : base(commandService,contextService)
        {
            _goodsBlockWarpQueryService = goodsBlockWarpQueryService;
        }

        #region 后台管理
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ListPage")]
        public BaseApiResponse ListPage([FromBody]ListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var pageSize = 20;
            var goodsBlockWarps = _goodsBlockWarpQueryService.All();
            var total = goodsBlockWarps.Count();

            goodsBlockWarps = goodsBlockWarps.OrderByDescending(x => x.Sort).Skip(pageSize * (request.Page - 1)).Take(pageSize);
            return new Api.Models.Response.GoodsBlockWarps.ListResponse
            {
                Total = total,
                GoodsBlockWarps = goodsBlockWarps.Select(x => new GoodsBlockWarp
                {
                    Id = x.Id,
                    Name = x.Name,
                    Style = x.Style.ToString(),
                    IsShow = x.IsShow,
                    Sort = x.Sort
                }).ToList()
            };
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Add")]
        public async Task<BaseApiResponse> Add([FromBody]AddGoodsBlockWarpRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new CreateGoodsBlockWarpCommand(
                GuidUtil.NewSequentialId(),
                request.Name,
                request.Style,
                request.GoodsBlocks,
                request.IsShow,
                request.Sort
                );
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令执行失败" };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Edit")]
        public async Task<BaseApiResponse> Edit([FromBody]EditGoodsBlockWarpRequest request)
        {
            request.CheckNotNull(nameof(request));
            //判断
            var goodsBlockWarp = _goodsBlockWarpQueryService.Find(request.Id);
            if (goodsBlockWarp == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该项目" };
            }

            var command = new UpdateGoodsBlockWarpCommand(
                request.Name,
                request.Style,
                request.GoodsBlocks,
                request.IsShow,
                request.Sort)
            {
                AggregateRootId = request.Id
            };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令执行失败" };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Delete")]
        public async Task<BaseApiResponse> Delete([FromBody]DeleteRequest  request)
        {
            //判断
            var goodsBlockWarp = _goodsBlockWarpQueryService.Find(request.Id);
            if (goodsBlockWarp == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该项目" };
            }
            //删除
            var command = new DeleteGoodsBlockWarpCommand
            {
                AggregateRootId = request.Id
            };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令执行失败" };
            }
            return new BaseApiResponse();
        }
        #endregion

        
    }
}
