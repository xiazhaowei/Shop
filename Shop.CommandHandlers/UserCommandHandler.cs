using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Users;
using Shop.Commands.Users.ExpressAddresses;
using Shop.Common;
using Shop.Domain.Models.Users;
using Shop.Domain.Models.Wallets;
using Shop.Domain.Services;
using System;

namespace Shop.CommandHandlers
{
    /// <summary>
    /// 用户领域模型 事件处理
    /// </summary>
    public class UserCommandHandler:
        ICommandHandler<CreateUserCommand>,
        ICommandHandler<SetMyParentCommand>,
        ICommandHandler<EditUserCommand>,
        ICommandHandler<UpdateNickNameCommand>,
        ICommandHandler<UpdatePasswordCommand>,
        ICommandHandler<UpdatePortraitCommand>,
        ICommandHandler<UpdateRegionCommand>,
        ICommandHandler<UpdateInfoCommand>,
        ICommandHandler<ClearUserParentCommand>,
        ICommandHandler<BindWeixinCommand>,
        ICommandHandler<AddExpressAddressCommand>,
        ICommandHandler<UpdateExpressAddressCommand>,
        ICommandHandler<RemoveExpressAddressCommand>,
        ICommandHandler<LockUserCommand>,
        ICommandHandler<UnLockUserCommand>,
        ICommandHandler<FreezeUserCommand>,
        ICommandHandler<UnFreezeUserCommand>,
        ICommandHandler<AcceptMyNewSpendingCommand>,//我的消费返还善心
        ICommandHandler<AcceptChildBenevolenceCommand>,
        ICommandHandler<AcceptMyStoreNewSaleCommand>,
        ICommandHandler<GetChildStoreSaleBenevolenceCommand>,
        ICommandHandler<GetChildStoreSaleCashCommand>,
        ICommandHandler<InvotedNewUserCommand>,
        ICommandHandler<AcceptChildUpdateOrderCommand>,
        ICommandHandler<AcceptNewUpdateOrderCommand>,
        ICommandHandler<AcceptChildGratefulAwardCommand>,
        ICommandHandler<MyParentCanGetBenevolenceCommand>
    {
        private readonly ILockService _lockService;
        private readonly RegisterUserMobileService _registerUserMobileService;

        public UserCommandHandler() { }
        /// <summary>
        /// IOC 构造函数注入
        /// </summary>
        /// <param name="lockService"></param>
        /// <param name="registerUserMobileService"></param>
        public UserCommandHandler(ILockService lockService, RegisterUserMobileService registerUserMobileService)
        {
            _lockService = lockService;
            _registerUserMobileService = registerUserMobileService;
        }

        #region handle Command
        public void Handle(ICommandContext context, CreateUserCommand command)
        {
            _lockService.ExecuteInLock(typeof(UserMobileIndex).Name, () =>
            {
                User parent = null;
                if (command.ParentId!=Guid.Empty)
                {
                    parent = context.Get<User>(command.ParentId);
                }
                //创建user 领域对象
                var user = new User(command.AggregateRootId, parent,new UserInfo(
                    command.Mobile,
                    command.NickName,
                    command.Portrait,
                    command.Gender,
                    command.Region,
                    command.Password,
                    command.WeixinId,
                    command.UnionId));

                //验证Mobile 的唯一性
                _registerUserMobileService.RegisterMobile(command.Id, user.Id, command.Mobile);
                //将领域对象添加到上下文中
                context.Add(user);
            });
        }

        public void Handle(ICommandContext context, SetMyParentCommand command)
        {
            User parent = null;
            if (command.ParentId != Guid.Empty)
            {
                parent = context.Get<User>(command.ParentId);
            }
            context.Get<User>(command.AggregateRootId).SetMyParent(parent);
        }

        public void Handle(ICommandContext context, UpdateNickNameCommand command)
        {
            //从上下文中获取User领域对象，然后直接调用领域对象的方法
            context.Get<User>(command.AggregateRootId).UpdateNickName(command.NickName);
        }

      

        public void Handle(ICommandContext context,UpdatePasswordCommand command)
        {
            context.Get<User>(command.AggregateRootId).UpdatePassword(command.Password);
        }

        public void Handle(ICommandContext context,UpdateGenderCommand command)
        {
            context.Get<User>(command.AggregateRootId).UpdateGender(command.Gender);
        }

        public void Handle(ICommandContext content,UpdatePortraitCommand command)
        {
            content.Get<User>(command.AggregateRootId).UpdatePortrait(command.Portrait);
        }
        public void Handle(ICommandContext context,UpdateRegionCommand command)
        {
            context.Get<User>(command.AggregateRootId).UpdateRegion(command.Region);
        }

    

        public void Handle(ICommandContext context,AddExpressAddressCommand command)
        {
            context.Get<User>(command.AggregateRootId).AddExpressAddress(new ExpressAddressInfo(
                command.Region,
                command.Address,
                command.Name,
                command.Mobile,
                command.Zip
                ));
        }

        public void Handle(ICommandContext context,UpdateExpressAddressCommand command)
        {
            context.Get<User>(command.AggregateRootId).UpdateExpressAddress(command.ExpressAddressId,
                new ExpressAddressInfo(
                command.Region,
                command.Address,
                command.Name,
                command.Mobile,
                command.Zip
                ));
        }

        public void Handle(ICommandContext context,RemoveExpressAddressCommand command)
        {
            context.Get<User>(command.AggregateRootId).RemoveExpressAddress(command.ExpressAddressId);
        }

    

        public void Handle(ICommandContext context,LockUserCommand command)
        {
            context.Get<User>(command.AggregateRootId).Lock();
        }
        public void Handle(ICommandContext context, UnLockUserCommand command)
        {
            context.Get<User>(command.AggregateRootId).UnLock();
        }

        public void Handle(ICommandContext context, AcceptMyNewSpendingCommand command)
        {
            var userId= context.Get<Wallet>(command.WalletId).GetOwnerId();
            context.Get<User>(userId).AcceptMyNewSpending(
                command.Amount,
                command.StoreAmount,
                command.Benevolence,
                command.HighProfitAmount);
        }

        public void Handle(ICommandContext context, AcceptChildBenevolenceCommand command)
        {
            context.Get<User>(command.AggregateRootId).AcceptChildBenevolence(command.Amount, command.ProfitAmount,command.HighProfitAmount,command.Level);
        }
        
        

        public void Handle(ICommandContext context, EditUserCommand command)
        {
            context.Get<User>(command.AggregateRootId).Edit(command.NickName, command.Gender,command.Role);
        }

        public void Handle(ICommandContext context, AcceptMyStoreNewSaleCommand command)
        {
            var userId = context.Get<Wallet>(command.StoreOwnerWalletId).GetOwnerId();
            context.Get<User>(userId).AcceptMyStoreNewSale(command.Amount);
        }

        public void Handle(ICommandContext context, GetChildStoreSaleBenevolenceCommand command)
        {
            context.Get<User>(command.AggregateRootId).AcceptChildStoreSaleBenevolence(command.Amount);
        }
        public void Handle(ICommandContext context, GetChildStoreSaleCashCommand command)
        {
            context.Get<User>(command.AggregateRootId).AcceptChildStoreSaleCash(command.Amount);
        }

        public void Handle(ICommandContext context, FreezeUserCommand command)
        {
            context.Get<User>(command.AggregateRootId).Freeze();
        }

        public void Handle(ICommandContext context, UnFreezeUserCommand command)
        {
            context.Get<User>(command.AggregateRootId).UnFreeze();
        }

        public void Handle(ICommandContext context, ClearUserParentCommand command)
        {
            context.Get<User>(command.AggregateRootId).ClearParent();
        }

        public void Handle(ICommandContext context, InvotedNewUserCommand command)
        {
            context.Get<User>(command.AggregateRootId).AcceptNewRecommend(command.UserId);
        }

        public void Handle(ICommandContext context, BindWeixinCommand command)
        {
            context.Get<User>(command.AggregateRootId).BindWeixin(command.WeixinId, command.UnionId);
        }

        public void Handle(ICommandContext context, AcceptChildUpdateOrderCommand command)
        {
            context.Get<User>(command.AggregateRootId).AcceptChildUpdateOrder(
                command.NewVipId,
                command.NewVipRole,
                command.GoodsCount,
                command.LeftAwardAmount,
                command.Level,
                command.UpdateOrderType);
        }

        public void Handle(ICommandContext context, AcceptNewUpdateOrderCommand command)
        {
            context.Get<User>(command.AggregateRootId).AcceptNewUpdateOrder(command.GoodsCount,command.UpdateOrderType);
        }

        public void Handle(ICommandContext context, AcceptChildGratefulAwardCommand command)
        {
            context.Get<User>(command.AggregateRootId).AcceptChildGratefulAward(command.Amount,command.Remark);
        }

        public void Handle(ICommandContext context, UpdateInfoCommand command)
        {
            context.Get<User>(command.AggregateRootId).UpdateInfo(command.NickName, command.Region, command.Portrait);
        }

        public void Handle(ICommandContext context, MyParentCanGetBenevolenceCommand command)
        {
            context.Get<User>(command.AggregateRootId).MyParentCanGetBenevolence(command.BenevolenceAmount);
        }



        #endregion
    }
}
