using ENode.Commanding;
using Shop.Commands.Stores;
using Shop.Domain.Models.Stores;

namespace Shop.CommandHandlers
{
    public class StoreCommandHandler:
        ICommandHandler<CreateStoreCommand>,
        ICommandHandler<CustomerUpdateStoreCommand>,
        ICommandHandler<UpdateStoreCommand>,
        ICommandHandler<UpdateReturnAddressCommand>,
        ICommandHandler<UpdateStoreStautsCommand>,

        ICommandHandler<UpdateSubjectCommand>,

        ICommandHandler<AcceptNewStoreOrderCommand>,
        ICommandHandler<SetAccessCodeCommand>,
        ICommandHandler<ResetTodayStatisticCommand>
    {

        public void Handle(ICommandContext context, CreateStoreCommand command)
        {
            //创建聚合跟对象
            var store = new Store(command.AggregateRootId, command.UserId, new StoreInfo(
                command.AccessCode,
                command.Name,
                command.Description,
                command.Region,
                command.Address
                ),
                new SubjectInfo(
                    command.SubjectName,
                    command.SubjectNumber,
                    command.SubjectPic));
            //添加到上下文
            context.Add(store);
        }
        public void Handle(ICommandContext context, CustomerUpdateStoreCommand command)
        {
            context.Get<Store>(command.AggregateRootId).CustomerUpdate(new StoreCustomerEditableInfo (
                command.Name,
                command.Description,
                command.Address
            ));
        }
        public void Handle(ICommandContext context, UpdateStoreCommand command)
        {
            context.Get<Store>(command.AggregateRootId).Update(new StoreEditableInfo(
                command.Name,
                command.Description,
                command.Address,
                command.Type
            ),
            command.IsLocked);
        }
        public void Handle(ICommandContext context, AcceptNewStoreOrderCommand command)
        {
            context.Get<Store>(command.AggregateRootId).AcceptNewOrder(context.Get<StoreOrder>(command.StoreOrderId));
        }

        public void Handle(ICommandContext context, SetAccessCodeCommand command)
        {
            context.Get<Store>(command.AggregateRootId).SetAccessCode(command.AccessCode);
        }

        public void Handle(ICommandContext context, UpdateStoreStautsCommand command)
        {
            context.Get<Store>(command.AggregateRootId).UpdateStatus(command.Status);
        }

        public void Handle(ICommandContext context, UpdateSubjectCommand command)
        {
            context.Get<Store>(command.AggregateRootId).UpdateSubjectInfo(new SubjectInfo(
                command.SubjectName,
                command.SubjectNumber,
                command.SubjectPic));
        }

        public void Handle(ICommandContext context, ResetTodayStatisticCommand command)
        {
            context.Get<Store>(command.AggregateRootId).ResetTodayStatistic();
        }

        public void Handle(ICommandContext context, UpdateReturnAddressCommand command)
        {
            context.Get<Store>(command.AggregateRootId).ReturnAddressUpdate(new ReturnAddressInfo(
                command.Name,
                command.Mobile,
                command.Address
                ));
        }
    }
}
