using ENode.Commanding;
using System;

namespace Shop.Commands.Goodses
{
    public class DeleteGoodsCommand:Command<Guid>
    {
        public DeleteGoodsCommand() { }
        public DeleteGoodsCommand(Guid id):base(id) { }
    }
}
