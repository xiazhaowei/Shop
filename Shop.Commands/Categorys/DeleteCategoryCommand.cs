using ENode.Commanding;
using System;

namespace Shop.Commands.Categorys
{
    public class DeleteCategoryCommand : Command<Guid>
    {
        public DeleteCategoryCommand() { }
        public DeleteCategoryCommand(Guid id):base(id)
        {
        }
    }
}
