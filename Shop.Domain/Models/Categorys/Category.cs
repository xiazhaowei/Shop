using ENode.Domain;
using Shop.Common.Enums;
using Shop.Domain.Events.Categorys;
using System;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Categorys
{
    /// <summary>
    /// 分类聚合跟
    /// </summary>
    public class Category: AggregateRoot<Guid>
    {
        private string _name;
        private string _url;
        private CategoryType _type;
        private string _thumb;
        private int _sort;
        private Guid _parentId;
        private bool _isShow;

        public Category(Guid id,
            Category parent,
            string name,
            string url,
            string thumb,
            CategoryType type,
            bool isShow,
            int sort):base(id)
        {
            name.CheckNotNullOrEmpty(nameof(name));
            url.CheckNotNullOrEmpty(nameof(url));
            ApplyEvent(new CategoryCreatedEvent(parent == null ? Guid.Empty : parent.Id,name, url,thumb,type,isShow,sort));
        }

        public void UpdateCategory(string name,string url,string thumb,CategoryType type,bool isShow,int sort)
        {
            name.CheckNotNullOrEmpty(nameof(name));
            url.CheckNotNullOrEmpty(nameof(url));
            ApplyEvent(new CategoryUpdatedEvent(name,url,thumb,type,isShow,sort));
        }

        public void Delete()
        {
            ApplyEvent(new CategoryDeletedEvent());
        }

        #region Handle

        
        private void Handle(CategoryCreatedEvent evnt)
        {
            _parentId = evnt.ParentId;
            _name = evnt.Name;
            _url = evnt.Url;
            _thumb = evnt.Thumb;
            _type = evnt.Type;
            _isShow = evnt.IsShow;
            _sort = evnt.Sort;
        }
        private void Handle(CategoryDeletedEvent evnt)
        {
            _name = null;
            _url = null;
            _thumb = null;
        }
        private void Handle(CategoryUpdatedEvent evnt)
        {
            _name = evnt.Name;
            _url = evnt.Url;
            _thumb = evnt.Thumb;
            _type = evnt.Type;
            _isShow = evnt.IsShow;
            _sort = evnt.Sort;
        }
        #endregion
    }
}
