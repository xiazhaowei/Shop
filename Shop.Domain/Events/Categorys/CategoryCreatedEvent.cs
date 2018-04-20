using ENode.Eventing;
using Shop.Common.Enums;
using System;

namespace Shop.Domain.Events.Categorys
{
    [Serializable]
    public class CategoryCreatedEvent:DomainEvent<Guid>
    {
        public Guid ParentId { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
        public string Thumb { get; private set; }
        public CategoryType Type { get; private set; }
        public bool IsShow { get; set; }
        public int Sort { get; private set; }


        public CategoryCreatedEvent() { }
        public CategoryCreatedEvent(Guid parentId,string name,string url,string thumb,CategoryType type,bool isShow,int sort)
        {
            ParentId = parentId;
            Name = name;
            Url = url;
            Thumb = thumb;
            Type = type;
            IsShow = isShow;
            Sort = sort;
        }
    }
}
