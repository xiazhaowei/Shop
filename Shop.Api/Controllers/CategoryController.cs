using ENode.Commanding;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Categorys;
using Shop.Api.Services;
using Shop.QueryServices;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Dtos = Shop.QueryServices.Dtos;

namespace Shop.Api.Controllers
{
    public class CategoryController:BaseApiController
    {
        private ICategoryQueryService _categoryQueryService;//Q 端

        public CategoryController(ICommandService commandService,IContextService contextService,
            ICategoryQueryService categoryQueryService) : base(commandService,contextService)
        {
            _categoryQueryService = categoryQueryService;
        }


        /// <summary>
        /// 获取类别树信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
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

                var childrens = _categoryQueryService.GetChildren(cat.Id).Where(x=>x.IsShow).OrderByDescending(x=>x.Sort);
                foreach (var child in childrens)
                {
                    node.Children.Add(getNodeData(child));
                }
                return node;
            };

            List<Dtos.Category> rootsCategory = _categoryQueryService.RootCategorys().Where(x=>x.IsShow).OrderByDescending(x=>x.Sort).ToList();
            List<object> nodes = rootsCategory.Select(getNodeData).ToList();

            return new CategoryTreeResponse
            {
                Categorys = nodes
            };
        }
        
    }
}