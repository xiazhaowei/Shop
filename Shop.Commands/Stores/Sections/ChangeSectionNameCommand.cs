using ENode.Commanding;
using System;


namespace Shop.Commands.Stores.Sections
{
    public class ChangeSectionCommand : Command<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        private ChangeSectionCommand() { }
        public ChangeSectionCommand(Guid id, string name, string description) : base(id)
        {
            Name = name;
            Description = description;
        }
    }
}
