using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Users;
using Shop.Domain.Events.Users.ExpressAddresses;
using System;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 获取领域事件更新读库 基于Dapper
    /// </summary>
    public class UserViewModelGenerator: BaseGenerator,
        IMessageHandler<UserCreatedEvent>,
        IMessageHandler<MyParentSetedEvent>,
        IMessageHandler<UserEditedEvent>,
        IMessageHandler<UserParentClearedEvent>,
        IMessageHandler<UserNickNameUpdatedEvent>,
        IMessageHandler<UserInfoUpdatedEvent>,
        IMessageHandler<UserPasswordUpdatedEvent>,
        IMessageHandler<UserPortraitUpdatedEvent>,
        IMessageHandler<UserRegionUpdatedEvent>,
        IMessageHandler<UserGenderUpdatedEvent>,
        IMessageHandler<UserBindedWeixinEvent>,
        IMessageHandler<ExpressAddressAddedEvent>,
        IMessageHandler<ExpressAddressRemovedEvent>,
        IMessageHandler<ExpressAddressUpdatedEvent>,
        IMessageHandler<UserLockedEvent>,
        IMessageHandler<UserUnLockedEvent>,
        IMessageHandler<UserFreezeEvent>,
        IMessageHandler<UserUnFreezeEvent>,
        IMessageHandler<UserRoleToPasserEvent>,
        IMessageHandler<UserRoleToVipPasserEvent>,
        IMessageHandler<UserRoleToDirectorEvent>
    {

        /// <summary>
        /// 处理创建用户事件
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(UserCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                var info = evnt.Info;
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    ParentId=evnt.ParentId,
                    WalletId =evnt.WalletId,
                    CartId=evnt.CartId,
                    Mobile = info.Mobile,
                    NickName = info.NickName,
                    Portrait = info.Portrait,
                    Password = info.Password,
                    Gender=info.Gender,
                    Region = info.Region,
                    Role=(int)UserRole.Consumer,
                    IsLocked = (int)UserLock.UnLocked,
                    IsFreeze = (int)Freeze.UnFreeze,
                    CreatedOn = evnt.Timestamp,
                    WeixinId=info.WeixinId,
                    UnionId = info.UnionId,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, ConfigSettings.UserTable);
            });
        }


        #region 基本信息

        /// <summary>
        /// 处理 更新昵称事件
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(UserNickNameUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    NickName = evnt.NickName,                    
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }
        /// <summary>
        /// 处理 更新头像事件
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(UserPortraitUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Portrait = evnt.Portrait,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }
        /// <summary>
        /// 处理 更新性别事件
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(UserGenderUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Gender = evnt.Gender,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }
        /// <summary>
        /// 处理 更新密码事件
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(UserPasswordUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Password = evnt.Password,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }
        /// <summary>
        /// 处理 更新地区事件
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(UserRegionUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Region = evnt.Region,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(UserLockedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    IsLocked = (int)UserLock.Locked,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(UserUnLockedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    IsLocked = (int)UserLock.UnLocked,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(UserFreezeEvent evnt)
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
                }, ConfigSettings.UserTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(UserUnFreezeEvent evnt)
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
                }, ConfigSettings.UserTable);
            });
        }
        #endregion

        #region 快递地址
        /// <summary>
        /// 处理 添加快递地址
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(ExpressAddressAddedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                //尽管是更新ExpressAddresssTable但是也要更新聚合跟，因为地址属于聚合跟的状态
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.InsertAsync(new
                    {
                        Id = evnt.ExpressAddressId,
                        Name = evnt.Info.Name,
                        Region = evnt.Info.Region,
                        Address = evnt.Info.Address,
                        Zip = evnt.Info.Zip,
                        Mobile = evnt.Info.Mobile,
                        UserId = evnt.AggregateRootId,
                        CreatedOn = evnt.Timestamp
                    }, ConfigSettings.ExpressAddressTable, transaction);
                }
            });
        }
        /// <summary>
        /// 处理 更新快递地址
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(ExpressAddressUpdatedEvent evnt)
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
                }, ConfigSettings.UserTable, transaction);

                if (effectedRows == 1)
                {
                    await connection.UpdateAsync(new
                    {
                        Name = evnt.Info.Name,
                        Region = evnt.Info.Region,
                        Address = evnt.Info.Address,
                        Zip=evnt.Info.Zip
                    }, new
                    {
                        UserId = evnt.AggregateRootId,
                        Id = evnt.ExpressAddressId
                    }, ConfigSettings.ExpressAddressTable, transaction);
                }
            });
        }
        /// <summary>
        /// 处理 删除更新地址
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(ExpressAddressRemovedEvent evnt)
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
                }, ConfigSettings.UserTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.DeleteAsync(new
                    {
                        UserId = evnt.AggregateRootId,
                        Id = evnt.ExpressAddressId
                    }, ConfigSettings.ExpressAddressTable, transaction);
                }
            });
        }


        #endregion

        #region 用户角色

        public Task<AsyncTaskResult> HandleAsync(UserRoleToPasserEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Role = (int)UserRole.Passer,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }
        
        
        public Task<AsyncTaskResult> HandleAsync(UserEditedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    NickName = evnt.NickName,
                    Gender=evnt.Gender,
                    Role=(int)evnt.Role,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(MyParentSetedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    ParentId = evnt.ParentId,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(UserParentClearedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    ParentId=Guid.Empty,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(UserBindedWeixinEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    WeixinId = evnt.WeixinId,
                    UnionId = evnt.UnionId,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(UserRoleToVipPasserEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Role = (int)UserRole.VipPasser,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(UserRoleToDirectorEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Role = (int)UserRole.Director,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(UserInfoUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    NickName = evnt.NickName,
                    Region=evnt.Region,
                    Portrait=evnt.Portrait,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.UserTable);
            });
        }
        #endregion
    }
}
