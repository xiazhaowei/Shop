using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.Sections
{
    public class CreateSectionCommand : Command<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        private CreateSectionCommand() { }
        public CreateSectionCommand(Guid id,string name, string description): base(id)
        {
            Name = name;
            Description = description;
        }
    }
}
