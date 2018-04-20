using ENode.Domain;
using Shop.Domain.Events.Stores.Sections;
using System;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Stores.Sections
{
    /// <summary>
    /// 店铺行业聚合跟
    /// </summary>
    public class Section : AggregateRoot<Guid>
    {
        private string _name;
        private string _description;

        public Section(Guid id, string name, string description): base(id)
        {
            name.CheckNotNullOrEmpty(nameof(name));
            description.CheckNotNullOrEmpty(nameof(description));
            if (name.Length > 128)
            {
                throw new Exception("行业名称长度不能超过128");
            }
            if (description.Length > 256)
            {
                throw new Exception("行业描述长度不能超过256");
            }
            ApplyEvent(new SectionCreatedEvent(name, description));
        }

        public void ChangeSection(string name, string description)
        {
            ApplyEvent(new SectionChangedEvent(name, description));
        }

        private void Handle(SectionChangedEvent evnt)
        {
            _name = evnt.Name;
            _description = evnt.Description;
        }
        private void Handle(SectionCreatedEvent evnt)
        {
            _name = evnt.Name;
            _description = evnt.Description;
        }
    }
}
