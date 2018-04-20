using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Goodses;
using Shop.Domain.Events.Goodses.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class GoodsViewModelGenerator:BaseGenerator,
        IMessageHandler<GoodsCreatedEvent>,
        IMessageHandler<GoodsStoreUpdatedEvent>,
        IMessageHandler<GoodsUpdatedEvent>,
        IMessageHandler<GoodsPublishedEvent>,
        IMessageHandler<GoodsUnpublishedEvent>,
        IMessageHandler<GoodsStatusUpdatedEvent>,
        IMessageHandler<GoodsSellOutUpdatedEvent>,

        IMessageHandler<GoodsParamsUpdatedEvent>,
        IMessageHandler<CommentStatisticInfoChangedEvent>,

        IMessageHandler<SpecificationReservedEvent>,
        IMessageHandler<SpecificationReservationCommittedEvent>,
        IMessageHandler<SpecificationReservationCancelledEvent>,
        
        IMessageHandler<SpecificationsAddedEvent>,
        IMessageHandler<SpecificationsUpdatedEvent>,
        IMessageHandler<SpecificationAddedEvent>,
        IMessageHandler<SpecificationUpdatedEvent>,
        IMessageHandler<SpecificationStockChangedEvent>,

        IMessageHandler<GoodsDeletedEvent>
        

    {
        public Task<AsyncTaskResult> HandleAsync(GoodsCreatedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var info = evnt.Info;
                var effectedRows = await connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    StoreId = evnt.StoreId,
                    Name = info.Name,
                    Description = info.Description,
                    Pics = info.Pics.ExpandAndToString("|"),
                    Price = info.Price,
                    OriginalPrice = info.OriginalPrice,

                    Benevolence = info.Benevolence,

                    Stock = info.Stock,
                    SellOut = info.SellOut,
                    IsPayOnDelivery = info.IsPayOnDelivery,
                    IsInvoice = info.IsInvoice,
                    Is7SalesReturn = info.Is7SalesReturn,
                    CreatedOn = evnt.Timestamp,
                    Sort = info.Sort,
                    Rate = 5,
                    RateCount = 0,
                    QualityRate = 5,
                    PriceRate = 5,
                    ExpressRate = 5,
                    DescribeRate = 5,
                    IsPublished = 0,
                    Status=(int)GoodsStatus.UnVerify,
                    RefusedReason="",
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, ConfigSettings.GoodsTable,transaction);

                
                var tasks = new List<Task>();
                //删除原来的记录
                tasks.Add(connection.DeleteAsync(new
                {
                    GoodsId = evnt.AggregateRootId
                }, ConfigSettings.GoodsPubCategorysTable, transaction));

                //插入新的记录
                foreach (var categoryId in evnt.CategoryIds)
                {
                    tasks.Add(connection.InsertAsync(new
                    {
                        Id = GuidUtil.NewSequentialId(),
                        GoodsId = evnt.AggregateRootId,
                        CategoryId = categoryId
                    }, ConfigSettings.GoodsPubCategorysTable, transaction));
                }
                Task.WaitAll(tasks.ToArray());
            });
        }
        public Task<AsyncTaskResult> HandleAsync(GoodsStoreUpdatedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var info = evnt.Info;
                var effectedRows = await connection.UpdateAsync(new
                {
                    Name = info.Name,
                    Description = info.Description,
                    Pics=info.Pics.ExpandAndToString("|"),
                    Price=info.Price,
                    OriginalPrice = info.OriginalPrice,
                    Sort = info.Sort,
                    Stock = info.Stock,
                    Is7SalesReturn=info.Is7SalesReturn,
                    IsInvoice=info.IsInvoice,
                    IsPayOnDelivery=info.IsPayOnDelivery,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable,transaction);

                //更新分类
                var tasks = new List<Task>();
                tasks.Add(connection.DeleteAsync(new
                {
                    GoodsId = evnt.AggregateRootId
                }, ConfigSettings.GoodsPubCategorysTable, transaction));
                //插入新的记录
                foreach (var categoryId in evnt.CategoryIds)
                {
                    tasks.Add(connection.InsertAsync(new
                    {
                        Id = GuidUtil.NewSequentialId(),
                        GoodsId = evnt.AggregateRootId,
                        CategoryId = categoryId
                    }, ConfigSettings.GoodsPubCategorysTable, transaction));
                }
                Task.WaitAll(tasks.ToArray());
                
            });
        }
        public Task<AsyncTaskResult> HandleAsync(GoodsUpdatedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var info = evnt.Info;
                var effectedRows = await connection.UpdateAsync(new
                {
                    Name = info.Name,
                    Description = info.Description,
                    Pics = info.Pics.ExpandAndToString("|"),
                    Price = info.Price,
                    Benevolence = info.Benevolence,
                    SellOut=info.SellOut,
                    Status=(int)info.Status,
                    RefusedReason = info.RefusedReason,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable, transaction);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(GoodsPublishedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    IsPublished = 1,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(GoodsUnpublishedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    IsPublished = 0,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(SpecificationReservedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable, transaction);

                if (effectedRows == 1)
                {
                    var tasks = new List<Task>();

                    //插入预定记录
                    foreach (var reservationItem in evnt.ReservationItems)
                    {
                        tasks.Add(connection.InsertAsync(new
                        {
                            GoodsId = evnt.AggregateRootId,
                            ReservationId = evnt.ReservationId,
                            SpecificationId = reservationItem.SpecificationId,
                            Quantity = reservationItem.Quantity
                        }, ConfigSettings.ReservationItemsTable, transaction));
                    }

                    //更新规格的可用数量
                    foreach (var specificationAvailableQuantity in evnt.SpecificationAvailableQuantities)
                    {
                        tasks.Add(connection.UpdateAsync(new
                        {
                            AvailableQuantity = specificationAvailableQuantity.AvailableQuantity
                        }, new
                        {
                            GoodsId = evnt.AggregateRootId,
                            Id = specificationAvailableQuantity.SpecificationId
                        }, ConfigSettings.SpecificationTable, transaction));
                    }

                    Task.WaitAll(tasks.ToArray());
                }
            });
        }
        //确认预定
        public Task<AsyncTaskResult> HandleAsync(SpecificationReservationCommittedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable, transaction);

                if (effectedRows == 1)
                {
                    var tasks = new List<Task>();

                    //删除预定记录
                    tasks.Add(connection.DeleteAsync(new
                    {
                        GoodsId = evnt.AggregateRootId,
                        ReservationId = evnt.ReservationId
                    }, ConfigSettings.ReservationItemsTable, transaction));

                    //更新规格的库存的数量
                    foreach (var specificationStock in evnt.SpecificationStocks)
                    {
                        tasks.Add(connection.UpdateAsync(new
                        {
                            Stock = specificationStock.Stock
                        }, new
                        {
                            GoodsId = evnt.AggregateRootId,
                            Id = specificationStock.SpecificationId
                        }, ConfigSettings.SpecificationTable, transaction));
                    }

                    Task.WaitAll(tasks.ToArray());
                }
            });
        }
        public Task<AsyncTaskResult> HandleAsync(SpecificationReservationCancelledEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable, transaction);

                if (effectedRows == 1)
                {
                    var tasks = new List<Task>();

                    //删除预定记录
                    tasks.Add(connection.DeleteAsync(new
                    {
                        GoodsId = evnt.AggregateRootId,
                        ReservationId = evnt.ReservationId
                    }, ConfigSettings.ReservationItemsTable, transaction));

                    //更新规格的可用数量
                    foreach (var specificationAvailableQuantity in evnt.SpecificationAvailableQuantities)
                    {
                        tasks.Add(connection.UpdateAsync(new
                        {
                            AvailableQuantity = specificationAvailableQuantity.AvailableQuantity
                        }, new
                        {
                            GoodsId = evnt.AggregateRootId,
                            Id = specificationAvailableQuantity.SpecificationId
                        }, ConfigSettings.SpecificationTable, transaction));
                    }

                    Task.WaitAll(tasks.ToArray());
                }
            });
        }
        public Task<AsyncTaskResult> HandleAsync(GoodsParamsUpdatedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable, transaction);

                if (effectedRows == 1)
                {
                    var tasks = new List<Task>();
                    //删除原来的参数记录
                    tasks.Add(connection.DeleteAsync(new
                    {
                        GoodsId = evnt.AggregateRootId
                    }, ConfigSettings.GoodsParamTable, transaction));

                    //插入新的参数记录
                    foreach (var goodsParam in evnt.GoodsParams)
                    {
                        tasks.Add(connection.InsertAsync(new
                        {
                            Id=GuidUtil.NewSequentialId(),
                            GoodsId = evnt.AggregateRootId,
                            Name = goodsParam.Name,
                            Value = goodsParam.Value
                        }, ConfigSettings.GoodsParamTable, transaction));
                    }
                    Task.WaitAll(tasks.ToArray());
                }
            });
        }
        public Task<AsyncTaskResult> HandleAsync(SpecificationsAddedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable, transaction);

                if (effectedRows == 1)
                {
                    var tasks = new List<Task>();
                    //删除原来的规格
                    tasks.Add(connection.DeleteAsync(new
                    {
                        GoodsId = evnt.AggregateRootId
                    }, ConfigSettings.SpecificationTable, transaction));

                    //插入新的规格记录
                    foreach (var specification in evnt.Specifications)
                    {
                        tasks.Add(connection.InsertAsync(new
                        {
                            Id = specification.Id,
                            GoodsId = evnt.AggregateRootId,
                            Name = specification.Info.Name,
                            Value = specification.Info.Value,
                            Price=specification.Info.Price,
                            OriginalPrice=specification.Info.OriginalPrice,
                            Benevolence = specification.Info.Benevolence,
                            Stock=specification.Stock,
                            Number = specification.Info.Number,
                            Thumb=specification.Info.Thumb,
                            BarCode=specification.Info.BarCode,
                            AvailableQuantity=specification.Stock
                        }, ConfigSettings.SpecificationTable, transaction));
                    }
                    Task.WaitAll(tasks.ToArray());
                }
            });
        }
        public Task<AsyncTaskResult> HandleAsync(SpecificationAddedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.InsertAsync(new
                    {
                        Id = evnt.SpecificationId,
                        GoodsId = evnt.AggregateRootId,
                        Name = evnt.SpecificationInfo.Name,
                        Value=evnt.SpecificationInfo.Value,
                        Thumb=evnt.SpecificationInfo.Thumb,
                        Price=evnt.SpecificationInfo.Price,
                        OriginalPrice=evnt.SpecificationInfo.OriginalPrice,
                        Benevolence=evnt.SpecificationInfo.Benevolence,
                        Number = evnt.SpecificationInfo.Number,
                        BarCode=evnt.SpecificationInfo.BarCode,
                        Stock = evnt.Stock,
                        AvailableQuantity = evnt.Stock,
                        
                    }, ConfigSettings.SpecificationTable, transaction);
                }
            });
        }
        public Task<AsyncTaskResult> HandleAsync(SpecificationUpdatedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable, transaction);

                if (effectedRows == 1)
                {
                    await connection.UpdateAsync(new
                    {
                        Name = evnt.SpecificationInfo.Name,
                        Value=evnt.SpecificationInfo.Value,
                        Price = evnt.SpecificationInfo.Price,
                        OriginalPrice=evnt.SpecificationInfo.OriginalPrice,
                        Benevolence=evnt.SpecificationInfo.Benevolence,
                        Number=evnt.SpecificationInfo.Number,
                        Thumb=evnt.SpecificationInfo.Thumb,
                        BarCode=evnt.SpecificationInfo.BarCode
                    }, new
                    {
                        GoodsId = evnt.AggregateRootId,
                        Id = evnt.SpecificationId
                    }, ConfigSettings.SpecificationTable, transaction);
                }
            });
        }
        public Task<AsyncTaskResult> HandleAsync(SpecificationStockChangedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence - 1
                }, ConfigSettings.GoodsTable, transaction);

                if (effectedRows == 1)
                {
                    await connection.UpdateAsync(new
                    {
                        Stock = evnt.Stock,
                        AvailableQuantity = evnt.AvailableQuantity
                    }, new
                    {
                        GoodsId = evnt.AggregateRootId,
                        Id = evnt.SpecificationId
                    }, ConfigSettings.SpecificationTable, transaction);
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(CommentStatisticInfoChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Rate = evnt.StatisticInfo.Rate,
                    RateCount = evnt.StatisticInfo.RateCount,
                    QualityRate = evnt.StatisticInfo.QualityRate,
                    PriceRate = evnt.StatisticInfo.PriceRate,
                    ExpressRate=evnt.StatisticInfo.ExpressRate,
                    DescribeRate=evnt.StatisticInfo.DescribeRate,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(GoodsStatusUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Status = (int)evnt.Status,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(SpecificationsUpdatedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable, transaction);

                if (effectedRows == 1)
                {
                    var tasks = new List<Task>();
                    //更新规格
                    foreach (var specification in evnt.Specifications)
                    {
                        tasks.Add(connection.UpdateAsync(new
                        {
                            Name = specification.Info.Name,
                            Value = specification.Info.Value,
                            Price = specification.Info.Price,
                            OriginalPrice = specification.Info.OriginalPrice,
                            Benevolence=specification.Info.Benevolence,
                            Stock = specification.Stock,
                            Number = specification.Info.Number,
                            Thumb = specification.Info.Thumb,
                            BarCode = specification.Info.BarCode,
                            AvailableQuantity = specification.Stock
                        },new {
                            Id=specification.Id,
                            GoodsId=evnt.AggregateRootId
                        }, ConfigSettings.SpecificationTable, transaction));
                    }
                    Task.WaitAll(tasks.ToArray());
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(GoodsDeletedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.DeleteAsync(new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable, transaction);

                if (effectedRows == 1)
                {
                    var tasks = new List<Task>();
                    //删除规格
                    tasks.Add(connection.DeleteAsync(new
                    {
                        GoodsId = evnt.AggregateRootId
                    }, ConfigSettings.SpecificationTable, transaction));
                    //删除参数
                    tasks.Add(connection.DeleteAsync(new
                    {
                        GoodsId = evnt.AggregateRootId
                    }, ConfigSettings.GoodsParamTable, transaction));
                    //删除所属分类
                    tasks.Add(connection.DeleteAsync(new
                    {
                        GoodsId = evnt.AggregateRootId
                    }, ConfigSettings.GoodsPubCategorysTable, transaction));
                    //删除评价
                    tasks.Add(connection.DeleteAsync(new
                    {
                        GoodsId = evnt.AggregateRootId
                    }, ConfigSettings.GoodsCommentsTable, transaction));

                    Task.WaitAll(tasks.ToArray());
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(GoodsSellOutUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    SellOut = evnt.FinallySellOut,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsTable);
            });
        }
    }
}
