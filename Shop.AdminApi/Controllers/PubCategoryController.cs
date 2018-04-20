using ENode.Commanding;
using Shop.AdminApi.Extensions;
using Shop.AdminApi.Services;
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
using System.Web;
using System.Web.Http;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.AdminApi.Controllers
{
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
        public async Task<BaseApiResponse> Add(AddPubCategoryRequest request)
        {
            request.CheckNotNull(nameof(request));

            var newpubcategoryid = GuidUtil.NewSequentialId();
            var command = new CreatePubCategoryCommand(
                newpubcategoryid,
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "添加发布分类", newpubcategoryid, request.Name);
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "删除发布分类", request.Id, category.Name);
            return new BaseApiResponse();
        }
        /// <summary>
        /// 编辑类别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Update(UpdatePubCategoryRequest request)
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "编辑发布分类", request.Id, request.Name);
            return new BaseApiResponse();
        }

        

        /// <summary>
        /// 获取类别树信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
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