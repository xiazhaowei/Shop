using ENode.Domain;
using Shop.Domain.Events.PubCategorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.PubCategorys
{
    /// <summary>
    /// 商品发布分类聚合跟
    /// </summary>
    public class PubCategory: AggregateRoot<Guid>
    {
        private string _name;
        private string _thumb;
        private int _sort;
        private Guid _parentId;
        private bool _isShow;

        
        public PubCategory(Guid id,PubCategory parent,string name,string thumb,bool isShow,int sort):base(id)
        {
            name.CheckNotNullOrEmpty(nameof(name));
            ApplyEvent(new PubCategoryCreatedEvent(parent == null ? Guid.Empty : parent.Id,name,thumb,isShow,sort));
        }

        public void UpdateCategory(string name,string thumb,bool isShow,int sort)
        {
            ApplyEvent(new PubCategoryUpdatedEvent(name,thumb,isShow,sort));
        }

        public void Delete()
        {
            ApplyEvent(new PubCategoryDeletedEvent());
        }


        #region Handle


        private void Handle(PubCategoryCreatedEvent evnt)
        {
            _parentId = evnt.ParentId;
            _name = evnt.Name;
            _thumb = evnt.Thumb;
            _isShow = evnt.IsShow;
            _sort = evnt.Sort;
        }
        private void Handle(PubCategoryDeletedEvent evnt)
        {
            _name = null;
            _thumb = null;
        }
        private void Handle(PubCategoryUpdatedEvent evnt)
        {
            _name = evnt.Name;
            _thumb = evnt.Thumb;
            _isShow = evnt.IsShow;
            _sort = evnt.Sort;
        }
        #endregion
    }
}
