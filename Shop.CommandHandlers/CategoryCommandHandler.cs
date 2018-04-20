using ENode.Commanding;
using Shop.Commands.Categorys;
using Shop.Domain.Models.Categorys;
using System;

namespace Shop.CommandHandlers
{
    public class CategoryCommandHandler :
        ICommandHandler<CreateCategoryCommand>,
        ICommandHandler<UpdateCategoryCommand>,
        ICommandHandler<DeleteCategoryCommand>
    {
        public void Handle(ICommandContext context, CreateCategoryCommand command)
        {
            Category parent = null;
            if (command.ParentId != Guid.Empty)
            {
                parent = context.Get<Category>(command.ParentId);
            }
            var category = new Category(
                command.AggregateRootId,
                parent, 
                command.Name,
                command.Url,
                command.Thumb,
                command.Type,
                command.IsShow,
                command.Sort);
            
            //将领域对象添加到上下文中
            context.Add(category);
        }

        public void Handle(ICommandContext context, UpdateCategoryCommand command)
        {
            context.Get<Category>(command.AggregateRootId).UpdateCategory(
                command.Name,
                command.Url,
                command.Thumb,
                command.Type,
                command.IsShow,
                command.Sort);
        }

        public void Handle(ICommandContext context, DeleteCategoryCommand command)
        {
            context.Get<Category>(command.AggregateRootId).Delete();
        }
    }
}
