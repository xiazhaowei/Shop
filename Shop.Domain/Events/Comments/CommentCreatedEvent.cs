using ENode.Eventing;
using Shop.Domain.Models.Comments;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Events.Comments
{
    [Serializable]
    public class CommentCreatedEvent : DomainEvent<Guid>
    {
        public CommentInfo Info { get; private set; }
        public RateInfo RateInfo { get; private set; }

        public CommentCreatedEvent() { }
        public CommentCreatedEvent(CommentInfo info,RateInfo rateInfo)
        {
            Info = info;
            RateInfo = rateInfo;
        }
    }
}
