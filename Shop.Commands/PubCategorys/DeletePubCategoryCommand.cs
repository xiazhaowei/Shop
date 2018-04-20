using ENode.Commanding;
using System;

namespace Shop.Commands.PubCategorys
{
    public class DeletePubCategoryCommand : Command<Guid>
    {
        public DeletePubCategoryCommand() { }
        public DeletePubCategoryCommand(Guid id):base(id)
        {
        }
    }
}
