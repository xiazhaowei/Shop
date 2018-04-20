using ENode.Commanding;
using Shop.Commands.Admins;
using Shop.Domain.Models.Admins;

namespace Shop.CommandHandlers
{
    public class AdminCommandHandler :
        ICommandHandler<CreateAdminCommand>,
        ICommandHandler<UpdateAdminCommand>,
        ICommandHandler<DeleteAdminCommand>,
        ICommandHandler<UpdatePasswordCommand>,
        ICommandHandler<NewOperatRecordCommand>
    {
        public void Handle(ICommandContext context, CreateAdminCommand command)
        {
            var admin = new Admin(
                command.AggregateRootId,
                new AdminInfo(
                command.Name,
                command.LoginName,
                command.Portrait,
                command.Password,
                command.Role,
                command.IsLocked));
            
            //将领域对象添加到上下文中
            context.Add(admin);
        }

        public void Handle(ICommandContext context, UpdateAdminCommand command)
        {
            context.Get<Admin>(command.AggregateRootId).Update(
                new AdminEditableInfo(
                    command.Name,
                    command.LoginName,
                    command.Portrait,
                    command.Role,
                    command.IsLocked));
        }

        public void Handle(ICommandContext context, DeleteAdminCommand command)
        {
            context.Get<Admin>(command.AggregateRootId).Delete();
        }

        public void Handle(ICommandContext context, UpdatePasswordCommand command)
        {
            context.Get<Admin>(command.AggregateRootId).UpdatePassword(command.Password);
        }

        public void Handle(ICommandContext context, NewOperatRecordCommand command)
        {
            context.Get<Admin>(command.AggregateRootId).AcceptOperatRecord(command.Operat, command.AboutId, command.Remark);
        }
    }
}
