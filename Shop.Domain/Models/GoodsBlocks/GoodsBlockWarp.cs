using ENode.Domain;
using Shop.Common.Enums;
using Shop.Domain.Events.GoodsBlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.GoodsBlocks
{
    public class GoodsBlockWarp: AggregateRoot<Guid>
    {
        private GoodsBlockWarpInfo _info;

        public GoodsBlockWarp(Guid id,GoodsBlockWarpInfo info):base(id)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new GoodsBlockWarpCreatedEvent(info));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        public void Update(GoodsBlockWarpInfo info)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new GoodsBlockWarpUpdatedEvent(info));
        }
        public void Delete()
        {
            ApplyEvent(new GoodsBlockWarpDeletedEvent());
        }

        #region Handle
        private void Handle(GoodsBlockWarpCreatedEvent evnt)
        {
            _info = evnt.Info;
        }

        private void Handle(GoodsBlockWarpUpdatedEvent evnt)
        {
            _info = evnt.Info;
        }
        private void Handle(GoodsBlockWarpDeletedEvent evnt)
        {
            _info = null;
        }
        #endregion
    }
}
