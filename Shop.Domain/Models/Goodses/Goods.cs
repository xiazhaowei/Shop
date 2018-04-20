using ENode.Domain;
using ENode.Eventing;
using Shop.Common.Enums;
using Shop.Domain.Events.Goodses;
using Shop.Domain.Events.Goodses.Specifications;
using Shop.Domain.Models.Comments;
using Shop.Domain.Models.Goodses.Specifications;
using Shop.Domain.Models.PublishableExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Goodses
{
    /// <summary>
    /// 商品聚合跟
    /// </summary>
    public class Goods: AggregateRoot<Guid>
    {
        private GoodsInfo _info;//基本信息
        private Guid _storeId;//所属店铺
        private IList<GoodsParam> _goodsParams;//商品参数
        private IList<Specification> _specifications;//商品规格 至少一个规格默认规格
        private IList<Guid> _categoryIds;//产品所属类别
        private ISet<Guid> _commentIds;//评价
        private CommentStatisticInfo _commentStatisticInfo;//评价统计信息
        private IDictionary<Guid, IEnumerable<ReservationItem>> _reservations;//预定信息防止超卖
        private bool _isPublished;//是否上架
        private GoodsStatus _status;//商品状态

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="info"></param>
        public Goods(Guid id,Guid storeId,IList<Guid> categoryIds, GoodsInfo info) : base(id)
        {
            ApplyEvent(new GoodsCreatedEvent(storeId, categoryIds, info));
        }

        /// <summary>
        /// 更新-更新基本信息-商家编辑
        /// </summary>
        /// <param name="info"></param>
        public void Update(IList<Guid> categoryIds,GoodsStoreEditableInfo info)
        {
            info.CheckNotNull(nameof(info));

            //if(info.Price!=_info.Price || info.OriginalPrice!=_info.OriginalPrice || info.Name!=_info.Name || info.Description!=_info.Description)
            //{
                //名称 价格 描述修改需要重启审核
                ApplyEvent(new GoodsStatusUpdatedEvent(GoodsStatus.UnVerify));
            //}
            ApplyEvent(new GoodsStoreUpdatedEvent(categoryIds, info));
        }
        /// <summary>
        /// 更新基本信息-管理编辑
        /// </summary>
        /// <param name="info"></param>
        public void AdminUpdate(GoodsEditableInfo info)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new GoodsUpdatedEvent(info));
        }

        /// <summary>
        /// 更新参数
        /// </summary>
        /// <param name="goodsParams"></param>
        public void UpdateParams(IList<GoodsParam> goodsParams)
        {
            goodsParams.CheckNotNull(nameof(goodsParams));
            ApplyEvent(new GoodsParamsUpdatedEvent(goodsParams));
        }
        /// <summary>
        /// 启用多规格
        /// </summary>
        /// <param name="goodSpecifications"></param>
        public void AddSpecifications(IList<Specification> specifications)
        {
            specifications.CheckNotNull(nameof(specifications));
            //判断是否有该商品的预定
            if (_reservations.Any())
            {
                throw new Exception("存在预定，不允许修改规格.");
            }
            ApplyEvent(new SpecificationsAddedEvent(specifications));
            ApplyEvent(new GoodsStatusUpdatedEvent(GoodsStatus.UnVerify));
        }
        /// <summary>
        /// 启用单规格 添加商品时自动启用
        /// </summary>
        /// <param name="specificationInfo"></param>
        /// <param name="stock"></param>
        public void AddSpecification(SpecificationInfo specificationInfo, int stock)
        {
            if (_reservations.Any())
            {
                throw new Exception("存在预定，不允许修改规格.");
            }
            ApplyEvent(new SpecificationAddedEvent(GuidUtil.NewSequentialId(), specificationInfo, stock));
        }
        /// <summary>
        /// 更新单规格
        /// </summary>
        /// <param name="SpecificationInfo"></param>
        /// <param name="stock"></param>
        public void UpdateSpecification(Guid specificationId, SpecificationInfo specificationInfo, int stock)
        {
            if (_reservations.Any())
            {
                throw new Exception("存在预定，不允许修改规格.");
            }

            var specification = _specifications.SingleOrDefault(x => x.Id == specificationId);
            if (specification == null)
            {
                throw new Exception("不存在该规格.");
            }
            
            if (specification.Stock != stock)
            {
                //如果库存修改
                var totalReservationQuantity = GetTotalReservationQuantity(specification.Id);
                if (stock < totalReservationQuantity)
                {
                    throw new Exception(string.Format("库存数量不能小于已预定数量:{0}", totalReservationQuantity));
                }
                ApplyEvent(new SpecificationStockChangedEvent(specificationId, stock, stock - totalReservationQuantity));
            }
            
            if(specification.Info.Price !=specificationInfo.Price || specification.Info.OriginalPrice != specificationInfo.OriginalPrice)
            {
                ApplyEvent(new GoodsStatusUpdatedEvent(GoodsStatus.UnVerify));
            }

            ApplyEvent(new SpecificationUpdatedEvent(specificationId, specificationInfo));
        }
        /// <summary>
        /// 更新规格，价格之类的-重启审核，管理员不重启审核
        /// </summary>
        /// <param name="specifications"></param>
        public void UpdateSpecifications(IList<Specification> specifications,bool isAdmin)
        {
            //判断是否有该商品的预定，有预定是不允许批量修改的
            if (_reservations.Any())
            {
                throw new Exception("存在预定，不允许修改规格.");
            }
            if(!isAdmin)
            {
                //非管理员修改要重启审核
                ApplyEvent(new GoodsStatusUpdatedEvent(GoodsStatus.UnVerify));
            }
            ApplyEvent(new SpecificationsUpdatedEvent(specifications));
        }

        /// <summary>
        /// 删除自己
        /// </summary>
        public void Delete()
        {
            if (_reservations.Any())
            {
                throw new Exception("商品存在预定，无法删除");
            }
            ApplyEvent(new GoodsDeletedEvent());
        }

        /// <summary>
        /// 上架
        /// </summary>
        public void Publish()
        {
            ApplyEvent(new GoodsPublishedEvent());
        }
        /// <summary>
        /// 下降
        /// </summary>
        public void Unpublish()
        {
            if (_reservations.Any())
            {
                throw new Exception("存在预定，不允许下架.");
            }
            
            ApplyEvent(new GoodsUnpublishedEvent());
        }

        /// <summary>
        /// 更改审核状态
        /// </summary>
        /// <param name="status"></param>
        public void UpdateStatus(GoodsStatus status)
        {
            if (_reservations.Any())
            {
                throw new Exception("存在预定，不允许修改状态.");
            }
            ApplyEvent(new GoodsStatusUpdatedEvent(status));
        }

        /// <summary>
        /// 接受新评价-更新商品评价得分
        /// </summary>
        /// <param name="comment"></param>
        public void AcceptNewComment(Comment comment)
        {
            if (!_commentIds.Add(comment.Id)) return;

            var commentInfo = comment.GetCommentInfo();
            var rateInfo = comment.GetRateInfo();
            if (_commentStatisticInfo == null)
            {
                //第一次评论
                ApplyEvent(new CommentStatisticInfoChangedEvent(new CommentStatisticInfo(
                   rateInfo.Rate,
                    rateInfo.PriceRate,
                    rateInfo.DescribeRate,
                    rateInfo.QualityRate,
                    rateInfo.ExpressRate,
                    1)));
            }
            else
            {
                //求平均重新评估Rate值
                ApplyEvent(new CommentStatisticInfoChangedEvent(new CommentStatisticInfo(
                    _commentStatisticInfo.Rate.Ave(rateInfo.Rate),
                    _commentStatisticInfo.PriceRate.Ave(rateInfo.PriceRate),
                    _commentStatisticInfo.DescribeRate.Ave(rateInfo.DescribeRate),
                    _commentStatisticInfo.QualityRate.Ave(rateInfo.QualityRate),
                    _commentStatisticInfo.ExpressRate.Ave(rateInfo.ExpressRate),
                    _commentStatisticInfo.RateCount + 1)));
            }
        }

        
        /// <summary>
        /// 预定
        /// </summary>
        /// <param name="reservationId">其实是订单ID</param>
        /// <param name="reservationItems"></param>
        public void MakeReservation(Guid reservationId, IEnumerable<ReservationItem> reservationItems)
        {
            if (!_isPublished)
            {
                throw new Exception("不能预定未上架的商品");
            }
            if (_reservations.ContainsKey(reservationId))
            {
                throw new Exception(string.Format("重复的订单，订单号:{0}", reservationId));
            }
            if (reservationItems == null || reservationItems.Count() == 0)
            {
                throw new Exception(string.Format("订单项目为空, 订单号:{0}", reservationId));
            }

            var specificationAvailableQuantities = new List<SpecificationAvailableQuantity>();
            foreach (var reservationItem in reservationItems)
            {
                if (reservationItem.Quantity <= 0)
                {
                    throw new Exception(string.Format("预定数量必须大于0, 订单号:{0}, 规格:{1}", reservationId, reservationItem.SpecificationId));
                }
                var specification = _specifications.SingleOrDefault(x => x.Id == reservationItem.SpecificationId);
                if (specification == null)
                {
                    throw new ArgumentOutOfRangeException(string.Format("商品规格 '{0}' 不存在.", reservationItem.SpecificationId));
                }
                var availableStock = specification.Stock - GetTotalReservationQuantity(specification.Id);
                if (availableStock < reservationItem.Quantity)
                {
                    //库存数量不足
                    throw new SpecificationInsufficientException(_id, reservationId);
                }
                specificationAvailableQuantities.Add(new SpecificationAvailableQuantity(specification.Id, availableStock - reservationItem.Quantity));
            }
            ApplyEvent(new SpecificationReservedEvent(reservationId, reservationItems, specificationAvailableQuantities));
        }

        /// <summary>
        /// 确认预定（已付款） 更新库存
        /// </summary>
        /// <param name="reservationId"></param>
        public void CommitReservation(Guid reservationId)
        {
            IEnumerable<ReservationItem> reservationItems;
            if (_reservations.TryGetValue(reservationId, out reservationItems))
            {
                var specificationStocks = new List<SpecificationStock>();
                int totalBuyCount = 0;
                foreach (var reservationItem in reservationItems)
                {
                    var specification = _specifications.Single(x => x.Id == reservationItem.SpecificationId);
                    totalBuyCount+= reservationItem.Quantity;
                    //减库存
                    specificationStocks.Add(new SpecificationStock(specification.Id, specification.Stock - reservationItem.Quantity));
                }
                ApplyEvent(new SpecificationReservationCommittedEvent(reservationId, specificationStocks));
                //更新商品的已售数量
                var finallySellOut = _info.SellOut + totalBuyCount;
                ApplyEvent(new GoodsSellOutUpdatedEvent(finallySellOut));
            }
        }
        /// <summary>
        /// 取消预定
        /// </summary>
        /// <param name="reservationId"></param>
        public void CancelReservation(Guid reservationId)
        {
            IEnumerable<ReservationItem> reservationItems;
            if (_reservations.TryGetValue(reservationId, out reservationItems))
            {
                var specificationAvailableQuantities = new List<SpecificationAvailableQuantity>();
                foreach (var reservationItem in reservationItems)
                {
                    var specification = _specifications.Single(x => x.Id == reservationItem.SpecificationId);
                    var availableQuantity = specification.Stock - GetTotalReservationQuantity(specification.Id);
                    specificationAvailableQuantities.Add(new SpecificationAvailableQuantity(specification.Id, availableQuantity + reservationItem.Quantity));
                }
                ApplyEvent(new SpecificationReservationCancelledEvent(reservationId, specificationAvailableQuantities));
            }
        }

        #region Handle
        private void Handle(GoodsCreatedEvent evnt)
        {
            _info = evnt.Info;
            _storeId = evnt.StoreId;
            _categoryIds = evnt.CategoryIds;
            _commentIds = new HashSet<Guid>();
            _goodsParams = new List<GoodsParam>();
            _specifications = new List<Specification>();
            _reservations = new Dictionary<Guid, IEnumerable<ReservationItem>>();
            _isPublished = false;
            _status = GoodsStatus.UnVerify;
        }
        private void Handle(GoodsStoreUpdatedEvent evnt)
        {
            _categoryIds = evnt.CategoryIds;
            var editableInfo = evnt.Info;
            _info = new GoodsInfo(
                editableInfo.Name,
                editableInfo.Description,
                editableInfo.Pics,
                editableInfo.Price,
                editableInfo.OriginalPrice,
                _info.Benevolence,
                editableInfo.Stock,
                _info.SellOut,
                editableInfo.IsPayOnDelivery,
                editableInfo.IsInvoice,
                editableInfo.Is7SalesReturn,
                editableInfo.Sort);
        }
        private void Handle(GoodsUpdatedEvent evnt)
        {
            var editableInfo = evnt.Info;
            _info = new GoodsInfo(
                editableInfo.Name,
                editableInfo.Description,
                editableInfo.Pics,
                editableInfo.Price,
                _info.OriginalPrice,
                editableInfo.Benevolence,
                _info.Stock,
                editableInfo.SellOut,
                _info.IsPayOnDelivery,
                _info.IsInvoice,
                _info.Is7SalesReturn,
                _info.Sort);
        }
        private void Handle(GoodsSellOutUpdatedEvent evnt)
        {
            _info.SellOut = evnt.FinallySellOut;
        }
        private void Handle(GoodsDeletedEvent evnt)
        {
            _categoryIds = null;
            _reservations = null;
            _info = null;
            _commentIds = null;
            _goodsParams = null;
            _specifications = null;
        }
        private void Handle(GoodsStatusUpdatedEvent evnt)
        {
            _status = evnt.Status;
        }
        private void Handle(GoodsPublishedEvent evnt)
        {
            _isPublished = true;
        }
        private void Handle(GoodsUnpublishedEvent evnt)
        {
            _isPublished = false;
        }
        private void Handle(GoodsParamsUpdatedEvent evnt)
        {
            _goodsParams = evnt.GoodsParams;
        }
        private void Handle(SpecificationsAddedEvent evnt)
        {
            _specifications = evnt.Specifications;
        }
        private void Handle(SpecificationsUpdatedEvent evnt)
        {
            _specifications = evnt.Specifications;
        }
        private void Handle(SpecificationAddedEvent evnt)
        {
            _specifications.Add(new Specification(evnt.SpecificationId, evnt.SpecificationInfo,evnt.Stock));
        }
        private void Handle(SpecificationUpdatedEvent evnt)
        {
            _specifications.Single(x => x.Id == evnt.SpecificationId).Info = evnt.SpecificationInfo;
        }
        private void Handle(SpecificationStockChangedEvent evnt)
        {
            _specifications.Single(x => x.Id == evnt.SpecificationId).Stock = evnt.Stock;
        }
        private void Handle(CommentStatisticInfoChangedEvent evnt)
        {
            _commentStatisticInfo = evnt.StatisticInfo;
        }
        private void Handle(SpecificationReservedEvent evnt)
        {
            _reservations.Add(evnt.ReservationId, evnt.ReservationItems);
        }
        private void Handle(SpecificationReservationCommittedEvent evnt)
        {
            _reservations.Remove(evnt.ReservationId);
            foreach (var specificationStocks in evnt.SpecificationStocks)
            {
                _specifications.Single(x => x.Id == specificationStocks.SpecificationId).Stock = specificationStocks.Stock;
            }
        }
        private void Handle(SpecificationReservationCancelledEvent evnt)
        {
            _reservations.Remove(evnt.ReservationId);
        }
        
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取某座位类型的所以预定数量
        /// </summary>
        /// <param name="seatTypeId"></param>
        /// <returns></returns>
        private int GetTotalReservationQuantity(Guid specificationId)
        {
            var totalReservationQuantity = 0;
            foreach (var reservation in _reservations)
            {
                var reservationItem = reservation.Value.SingleOrDefault(x => x.SpecificationId ==specificationId);
                if (reservationItem != null)
                {
                    totalReservationQuantity += reservationItem.Quantity;
                }
            }
            return totalReservationQuantity;
        }

        #endregion
    }
}
