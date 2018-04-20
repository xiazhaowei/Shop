using ENode.Commanding;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.PubCategorys;
using Shop.Api.Services;
using Shop.QueryServices;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace Shop.Api.Controllers
{
    public class PubCategoryController:BaseApiController
    {
        private IPubCategoryQueryService _pubCategoryQueryService;//Q 端

        public PubCategoryController(ICommandService commandService, IContextService contextService,
            IPubCategoryQueryService pubCategoryQueryService) : base(commandService,contextService)
        {
            _pubCategoryQueryService = pubCategoryQueryService;
        }

        /// <summary>
        /// 获取类别树信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
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

                var childrens = _pubCategoryQueryService.GetChildren(cat.Id).Where(x=>x.IsShow).OrderByDescending(x=>x.Sort);
                foreach (var child in childrens)
                {
                    node.Children.Add(getNodeData(child));
                }
                return node;
            };

            List<PubCategory> rootsCategory = _pubCategoryQueryService.RootCategorys().Where(x=>x.IsShow).OrderByDescending(x=>x.Sort).ToList();
            List<object> nodes = rootsCategory.Select(getNodeData).ToList();

            return new PubCategoryTreeResponse
            {
                Categorys = nodes
            };
        }
        
    }
}