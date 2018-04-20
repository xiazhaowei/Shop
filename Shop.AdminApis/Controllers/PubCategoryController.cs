using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.PubCategorys;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.PubCategorys;
using Shop.Commands.PubCategorys;
using Shop.QueryServices;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
    public class PubCategoryController:BaseApiController
    {
        private IPubCategoryQueryService _pubCategoryQueryService;//Q 端

        public PubCategoryController(ICommandService commandService, IContextService contextService,
            IPubCategoryQueryService pubCategoryQueryService) : base(commandService,contextService)
        {
            _pubCategoryQueryService = pubCategoryQueryService;
        }
        
        #region 管理


        /// <summary>
        /// 添加类别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Add")]
        public async Task<BaseApiResponse> Add([FromBody]AddPubCategoryRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new CreatePubCategoryCommand(
                GuidUtil.NewSequentialId(),
                request.ParentId,
                request.Name,
                request.Thumb,
                request.IsShow,
                request.Sort);

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();

        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Delete")]
        public async Task<BaseApiResponse> Delete([FromBody]DeleteRequest request)
        {
            request.CheckNotNull(nameof(request));
            //分类判断
            var category = _pubCategoryQueryService.Find(request.Id);
            if (category == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该分类" };
            }
            //判断是否有子分类
            var children = _pubCategoryQueryService.GetChildren(request.Id);
            if (children.Any())
            {
                return new BaseApiResponse { Code = 400, Message = "包含子分类，无法删除" };
            }
            //删除
            var command = new DeletePubCategoryCommand(request.Id);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        /// <summary>
        /// 编辑类别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Update")]
        public async Task<BaseApiResponse> Update([FromBody]UpdatePubCategoryRequest request)
        {
            request.CheckNotNull(nameof(request));
            var command = new UpdatePubCategoryCommand(
                request.Name,
                request.Thumb,
                request.IsShow,
                request.Sort)
            {
                AggregateRootId = request.Id
            };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        

        /// <summary>
        /// 获取类别树信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("CategoryTree")]
        public BaseApiResponse CategoryTree()
        {
            //递归获取分类包含子类
            Func<PubCategory, object> getNodeData = null;

            getNodeData = cat => {
                dynamic node = new ExpandoObject();
                node.Id = cat.Id;
                node.Name = cat.Name;
                node.Thumb = cat.Thumb;
                node.IsShow = cat.IsShow;
                node.Sort = cat.Sort;
                node.Children = new List<dynamic>();

                var childrens = _pubCategoryQueryService.GetChildren(cat.Id).OrderByDescending(x=>x.Sort);
                foreach (var child in childrens)
                {
                    node.Children.Add(getNodeData(child));
                }
                return node;
            };

            List<PubCategory> rootsCategory = _pubCategoryQueryService.RootCategorys().OrderByDescending(x=>x.Sort).ToList();
            List<object> nodes = rootsCategory.Select(getNodeData).ToList();

            return new PubCategoryTreeResponse
            {
                Categorys = nodes
            };
        }
        
        #endregion

        
    }
}