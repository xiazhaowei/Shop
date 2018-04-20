using ENode.Commanding;
using Shop.AdminApi.Extensions;
using Shop.AdminApi.Services;
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
using System.Web;
using System.Web.Http;
using Xia.Common;
using Xia.Common.Extensions;
using Dtos = Shop.QueryServices.Dtos;

namespace Shop.AdminApi.Controllers
{
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
        public async Task<BaseApiResponse> Add(AddCategoryRequest request)
        {
            request.CheckNotNull(nameof(request));
            var newcategoryid = GuidUtil.NewSequentialId();
            var command = new CreateCategoryCommand(
                newcategoryid,
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "添加分类", newcategoryid, request.Name);

            return new BaseApiResponse();

        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Delete(DeleteRequest request)
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

            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "删除分类", request.Id, category.Name);

            return new BaseApiResponse();
        }
        /// <summary>
        /// 编辑类别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Update(UpdateCategoryRequest request)
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "编辑分类", request.Id, request.Name);
            return new BaseApiResponse();
        }
        #endregion

        
    }
}