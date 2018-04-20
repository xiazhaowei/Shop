using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.Categorys;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Categorys;
using Shop.Commands.Categorys;
using Shop.QueryServices;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;
using Dtos = Shop.QueryServices.Dtos;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
    public class CategoryController : BaseApiController
    {
        private ICategoryQueryService _categoryQueryService;//Q 端

        public CategoryController(ICommandService commandService, IContextService contextService,
            ICategoryQueryService categoryQueryService) : base(commandService, contextService)
        {
            _categoryQueryService = categoryQueryService;
        }


        #region 管理
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
            Func<Dtos.Category, object> getNodeData = null;

            getNodeData = cat => {
                dynamic node = new ExpandoObject();
                node.Id = cat.Id;
                node.Name = cat.Name;
                node.Thumb = cat.Thumb;
                node.Url = cat.Url;
                node.Type = cat.Type.ToString();
                node.IsShow = cat.IsShow;
                node.Sort = cat.Sort;
                node.Children = new List<dynamic>();

                var childrens = _categoryQueryService.GetChildren(cat.Id).OrderByDescending(x => x.Sort);
                foreach (var child in childrens)
                {
                    node.Children.Add(getNodeData(child));
                }
                return node;
            };

            List<Dtos.Category> rootsCategory = _categoryQueryService.RootCategorys().OrderByDescending(x => x.Sort).ToList();
            List<object> nodes = rootsCategory.Select(getNodeData).ToList();

            return new CategoryTreeResponse
            {
                Categorys = nodes
            };
        }

        /// <summary>
        /// 添加类别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Add")]
        public async Task<BaseApiResponse> Add([FromBody]AddCategoryRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new CreateCategoryCommand(
                GuidUtil.NewSequentialId(),
                request.ParentId,
                request.Name,
                request.Url,
                request.Thumb,
                request.Type,
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
            var category = _categoryQueryService.Find(request.Id);
            if(category==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该分类" };
            }
            //判断是否有子分类
            var children = _categoryQueryService.GetChildren(request.Id);
            if (children.Any())
            {
                return new BaseApiResponse { Code = 400, Message = "包含子分类，无法删除" };
            }
            //删除
            var command = new DeleteCategoryCommand(request.Id);
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
        public async Task<BaseApiResponse> Update([FromBody]UpdateCategoryRequest request)
        {
            request.CheckNotNull(nameof(request));
            var command = new UpdateCategoryCommand(
                request.Name,
                request.Url,
                request.Thumb,
                request.Type,
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
        #endregion

        
    }
}