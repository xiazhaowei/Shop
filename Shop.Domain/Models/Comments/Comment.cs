using System;
using ENode.Domain;
using System.Collections.Generic;
using Shop.Common;
using Xia.Common.Extensions;
using System.Linq;
using Shop.Domain.Events.Comments;

namespace Shop.Domain.Models.Comments
{
    /// <summary>
    /// 商品评价聚合跟
    /// </summary>
    public class Comment : AggregateRoot<Guid>
    {
        private CommentInfo _info;
        private RateInfo _rateInfo;

        public Comment(Guid id, CommentInfo info,RateInfo rateInfo): base(id)
        {
            info.CheckNotNull(nameof(info));
            rateInfo.CheckNotNull(nameof(rateInfo));

            ApplyEvent(new CommentCreatedEvent(info,rateInfo));
        }

        public CommentInfo GetCommentInfo()
        {
            return _info;
        }
        public RateInfo GetRateInfo()
        {
            return _rateInfo;
        }

        #region Handle
        private void Handle(CommentCreatedEvent evnt)
        {
            _info = evnt.Info;
            _rateInfo = evnt.RateInfo;
        }
        #endregion

    }
}
