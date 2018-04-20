using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Wallets;
using Shop.Domain.Events.Wallets.BankCards;
using Shop.Domain.Events.Wallets.RechargeApplys;
using Shop.Domain.Events.Wallets.WithdrawApplys;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 获取领域事件更新读库 基于Dapper
    /// </summary>
    public class WalletViewModelGenerator: BaseGenerator,
        IMessageHandler<WalletCreatedEvent>,

        IMessageHandler<NewCashTransferAcceptedEvent>,
        IMessageHandler<NewShopCashTransferAcceptedEvent>,
        IMessageHandler<NewBenevolenceTransferAcceptedEvent>,

        IMessageHandler<WalletAccessCodeUpdatedEvent>,

        IMessageHandler<BankCardAddedEvent>,
        IMessageHandler<BankCardRemovedEvent>,
        IMessageHandler<BankCardUpdatedEvent>,

        IMessageHandler<WithdrawApplyCreatedEvent>,
        IMessageHandler<WithdrawApplySuccessEvent>,
        IMessageHandler<WithdrawApplyRejectedEvent>,

        IMessageHandler<RechargeApplyCreatedEvent>,
        IMessageHandler<RechargeApplyStatusChangedEvent>,

        IMessageHandler<WithdrawStatisticInfoChangedEvent>,
        IMessageHandler<StatisticInfoChangedEvent>,
        IMessageHandler<WalletFreezeEvent>,
        IMessageHandler<WalletUnFreezeEvent>
        
    {
        public Task<AsyncTaskResult> HandleAsync(WalletCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    UserId = evnt.UserId,
                    AccessCode="",//交易密码默认为空，需用户后期自己设置
                    Cash = 0,
                    ShopCash = 0,
                    LockedCash=0,
                    Benevolence =0,
                    YesterdayEarnings=0,
                    Earnings=0,
                    YesterdayIndex=0,
                    BenevolenceTotal=0,
                    TodayBenevolenceAdded=0,
                    CreatedOn = evnt.Timestamp,
                    UpdatedOn=evnt.Timestamp,

                    TodayWithdrawAmount =0,
                    WeekWithdrawAmount = 0,
                    WithdrawTotalAmount = 0,
                    LastWithdrawTime = evnt.Timestamp,
                    IsFreeze=(int)Freeze.UnFreeze,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.WalletTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(NewCashTransferAcceptedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {

                return connection.UpdateAsync(new
                {
                    Cash = evnt.FinallyValue,
                    YesterdayEarnings = evnt.StatisticInfo.YesterdayEarnings,
                    Earnings = evnt.StatisticInfo.Earnings,
                    YesterdayIndex = evnt.StatisticInfo.YesterdayIndex,
                    BenevolenceTotal = evnt.StatisticInfo.BenevolenceTotal,
                    TodayBenevolenceAdded = evnt.StatisticInfo.TodayBenevolenceAdded,
                    UpdatedOn=evnt.StatisticInfo.UpdatedOn,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    UserId=evnt.UserId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.WalletTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(NewShopCashTransferAcceptedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {

                return connection.UpdateAsync(new
                {
                    ShopCash = evnt.FinallyValue,
                    YesterdayEarnings = evnt.StatisticInfo.YesterdayEarnings,
                    Earnings = evnt.StatisticInfo.Earnings,
                    YesterdayIndex = evnt.StatisticInfo.YesterdayIndex,
                    BenevolenceTotal = evnt.StatisticInfo.BenevolenceTotal,
                    TodayBenevolenceAdded = evnt.StatisticInfo.TodayBenevolenceAdded,
                    UpdatedOn = evnt.StatisticInfo.UpdatedOn,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    UserId = evnt.UserId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.WalletTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(NewBenevolenceTransferAcceptedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Benevolence = evnt.FinallyValue,
                    YesterdayEarnings = evnt.StatisticInfo.YesterdayEarnings,
                    Earnings = evnt.StatisticInfo.Earnings,
                    YesterdayIndex = evnt.StatisticInfo.YesterdayIndex,
                    BenevolenceTotal = evnt.StatisticInfo.BenevolenceTotal,
                    TodayBenevolenceAdded = evnt.StatisticInfo.TodayBenevolenceAdded,
                    UpdatedOn = evnt.StatisticInfo.UpdatedOn,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    UserId = evnt.UserId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.WalletTable);
            });
        }
        


        #region 银行卡
        /// <summary>
        /// 处理 添加
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(BankCardAddedEvent evnt)
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
                }, ConfigSettings.WalletTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.InsertAsync(new
                    {
                        Id = evnt.BankCardId,
                        BankName = evnt.Info.BankName,
                        OwnerName=evnt.Info.OwnerName,
                        Number = evnt.Info.Number,
                        WalletId = evnt.AggregateRootId,
                        CreatedOn=evnt.Timestamp
                    }, ConfigSettings.BankCardTable, transaction);
                }
            });
        }
        /// <summary>
        /// 处理 更新
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(BankCardUpdatedEvent evnt)
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
                }, ConfigSettings.WalletTable, transaction);

                if (effectedRows == 1)
                {
                    await connection.UpdateAsync(new
                    {
                        BankName = evnt.Info.BankName,
                        OwnerName=evnt.Info.OwnerName,
                        Number = evnt.Info.Number,
                    }, new
                    {
                        WalletId = evnt.AggregateRootId,
                        Id = evnt.BankCardId
                    }, ConfigSettings.BankCardTable, transaction);
                }
            });
        }
        /// <summary>
        /// 处理 删除
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(BankCardRemovedEvent evnt)
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
                }, ConfigSettings.WalletTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.DeleteAsync(new
                    {
                        WalletId = evnt.AggregateRootId,
                        Id = evnt.BankCardId
                    }, ConfigSettings.BankCardTable, transaction);
                }
            });
        }




        #endregion

        public Task<AsyncTaskResult> HandleAsync(WalletAccessCodeUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    AccessCode = evnt.AccessCode,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.WalletTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(WithdrawApplyCreatedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Cash=evnt.FinalCash,
                    LockedCash=evnt.FinalLockedCash,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.WalletTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.InsertAsync(new
                    {
                        Id = evnt.WithdrawApplyId,
                        WalletId = evnt.AggregateRootId,
                        Amount =evnt.Info.Amount,
                        BankName = evnt.Info.BankName,
                        BankNumber=evnt.Info.BankNumber,
                        BankOwner = evnt.Info.BankOwner,
                        Remark=evnt.Info.Remark,
                        CreatedOn = evnt.Timestamp,
                        Status=(int)WithdrawApplyStatus.Placed
                    }, ConfigSettings.WithdrawApplysTable, transaction);
                }
            });
        }
        public Task<AsyncTaskResult> HandleAsync(WithdrawApplySuccessEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    LockedCash = evnt.FinalLockedCash,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.WalletTable, transaction);

                if (effectedRows == 1)
                {
                    await connection.UpdateAsync(new
                    {
                        Status = (int)WithdrawApplyStatus.Success
                    }, new
                    {
                        WalletId = evnt.AggregateRootId,
                        Id = evnt.WithdrawApplyId
                    }, ConfigSettings.WithdrawApplysTable, transaction);
                }
            });
        }
        public Task<AsyncTaskResult> HandleAsync(WithdrawApplyRejectedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    LockedCash = evnt.FinalLockedCash,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.WalletTable, transaction);

                if (effectedRows == 1)
                {
                    await connection.UpdateAsync(new
                    {
                        Status = (int)WithdrawApplyStatus.Rejected,
                        Remark = evnt.Remark,
                    }, new
                    {
                        WalletId = evnt.AggregateRootId,
                        Id = evnt.WithdrawApplyId
                    }, ConfigSettings.WithdrawApplysTable, transaction);
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(RechargeApplyCreatedEvent evnt)
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
                }, ConfigSettings.WalletTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.InsertAsync(new
                    {
                        Id = evnt.RechargeApplyId,
                        WalletId = evnt.AggregateRootId,
                        Amount = evnt.Info.Amount,
                        Pic=evnt.Info.Pic,
                        BankName = evnt.Info.BankName,
                        BankNumber = evnt.Info.BankNumber,
                        BankOwner = evnt.Info.BankOwner,
                        Remark = evnt.Info.Remark,
                        CreatedOn = evnt.Timestamp,
                        Status = (int)evnt.Status
                    }, ConfigSettings.RechargeApplysTable, transaction);
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(RechargeApplyStatusChangedEvent evnt)
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
                }, ConfigSettings.WalletTable, transaction);

                if (effectedRows == 1)
                {
                    await connection.UpdateAsync(new
                    {
                        Status = (int)evnt.Status,
                        Remark = evnt.Remark,
                    }, new
                    {
                        WalletId = evnt.AggregateRootId,
                        Id = evnt.RechargeApplyId
                    }, ConfigSettings.RechargeApplysTable, transaction);
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(WithdrawStatisticInfoChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    TodayWithdrawAmount = evnt.Info.TodayWithdrawAmount,
                    WeekWithdrawAmount = evnt.Info.WeekWithdrawAmount,
                    WithdrawTotalAmount = evnt.Info.WithdrawTotalAmount,
                    LastWithdrawTime = evnt.Info.LastWithdrawTime,

                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.WalletTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(StatisticInfoChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    YesterdayEarnings = evnt.Info.YesterdayEarnings,
                    TodayBenevolenceAdded = evnt.Info.TodayBenevolenceAdded,
                    UpdatedOn = evnt.Info.UpdatedOn,

                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.WalletTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(WalletFreezeEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    IsFreeze = (int)Freeze.Freeze,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.WalletTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(WalletUnFreezeEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    IsFreeze = (int)Freeze.UnFreeze,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.WalletTable);
            });
        }
    }
}
