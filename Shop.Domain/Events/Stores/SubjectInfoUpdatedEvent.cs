using ENode.Eventing;
using Shop.Domain.Models.Stores;
using System;

namespace Shop.Domain.Events.Stores
{
    [Serializable]
    public class SubjectInfoUpdatedEvent:DomainEvent<Guid>
    {
        public SubjectInfo SubjectInfo { get; private set; }

        public SubjectInfoUpdatedEvent() { }
        public SubjectInfoUpdatedEvent(SubjectInfo subjectInfo)
        {
            SubjectInfo = subjectInfo;
        }
    }
}
