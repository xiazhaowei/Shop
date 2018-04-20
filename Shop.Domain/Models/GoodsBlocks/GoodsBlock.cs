using ENode.Domain;
using Shop.Domain.Events.GoodsBlocks;
using System;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.GoodsBlocks
{
    public class GoodsBlock: AggregateRoot<Guid>
    {
        private GoodsBlockInfo _info;

        public GoodsBlock(Guid id,GoodsBlockInfo info):base(id)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new GoodsBlockCreatedEvent(info));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        public void Update(GoodsBlockInfo info)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new GoodsBlockUpdatedEvent(info));
        }

        public void Delete()
        {
            ApplyEvent(new GoodsBlockDeletedEvent());
        }


        #region Handler
        private void Handle(GoodsBlockCreatedEvent evnt)
        {
            _info = evnt.Info;
        }

        private void Handle(GoodsBlockUpdatedEvent evnt)
        {
            _info = evnt.Info;
        }

        private void Handle(GoodsBlockDeletedEvent evnt)
        {
            _info = null;
        }
        #endregion
    }
}
