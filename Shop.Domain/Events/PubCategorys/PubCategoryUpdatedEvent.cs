using ENode.Eventing;
using System;

namespace Shop.Domain.Events.PubCategorys
{
    [Serializable]
    public class PubCategoryUpdatedEvent:DomainEvent<Guid>
    {
        public string Name { get; private set; }
        public string Thumb { get; private set; }
        public bool IsShow { get; private set; }
        public int Sort { get; private set; }

        public PubCategoryUpdatedEvent() { }
        public PubCategoryUpdatedEvent(string name,string thumb,bool isShow,int sort)
        {
            Name = name;
            Thumb = thumb;
            IsShow = isShow;
            Sort = sort;
        }
    }
}
