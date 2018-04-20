using ENode.Commanding;
using Shop.Commands.OfflineStores;
using Shop.Domain.Models.OfflineStores;
using Shop.Domain.Models.Users;

namespace Shop.CommandHandlers
{
    public class OfflineStoreCommandHandler:
        ICommandHandler<CreateOfflineStoreCommand>,
        ICommandHandler<UpdateOfflineStoreCommand>,
        ICommandHandler<AcceptNewSaleCommand>,
        ICommandHandler<ResetTodayStatisticCommand>,
        ICommandHandler<DeleteOfflineStoreCommand>
    {

        public void Handle(ICommandContext context, CreateOfflineStoreCommand command)
        {
            //创建聚合跟对象
            var store = new OfflineStore(command.AggregateRootId, 
                command.UserId, 
                new OfflineStoreInfo(
                    command.Name,
                    command.Thumb,
                    command.Phone,
                    command.Region,
                    command.Address,
                    command.Description,
                    command.Labels,
                    command.Persent,
                    command.Longitude,
                    command.Latitude,
                    command.IsLocked
                ));
            //添加到上下文
            context.Add(store);
        }

        public void Handle(ICommandContext context, UpdateOfflineStoreCommand command)
        {
            context.Get<OfflineStore>(command.AggregateRootId).Update(new OfflineStoreInfo(
                command.Name,
                command.Thumb,
                command.Phone,
                command.Region,
                command.Address,
                command.Description,
                command.Labels,
                command.Persent,
                command.Longitude,
                command.Latitude,
                command.IsLocked
                ));
        }

        public void Handle(ICommandContext context, AcceptNewSaleCommand command)
        {
            var userWalletId= context.Get<User>(command.UserId).GetWalletId();

            var offlineStore = context.Get<OfflineStore>(command.AggregateRootId);
            var storeOwnerWalletId = context.Get<User>(offlineStore.GetOwnerId()).GetWalletId();

            offlineStore.AcceptNewSale(storeOwnerWalletId, command.UserId,userWalletId, command.Amount);
        }

        public void Handle(ICommandContext context, ResetTodayStatisticCommand command)
        {
            context.Get<OfflineStore>(command.AggregateRootId).ResetTodayStatistic();
        }

        public void Handle(ICommandContext context, DeleteOfflineStoreCommand command)
        {
            context.Get<OfflineStore>(command.AggregateRootId).Delete();
        }
    }
}
