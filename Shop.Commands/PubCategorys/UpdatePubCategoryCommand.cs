using ENode.Commanding;
using System;

namespace Shop.Commands.PubCategorys
{
    public class UpdatePubCategoryCommand : Command<Guid>
    {
        public string Name { get; private set; }
        public string Thumb { get; private set; }
        public bool IsShow { get; private set; }
        public int Sort { get;private set; }

        public UpdatePubCategoryCommand() { }
        public UpdatePubCategoryCommand(string name,string thumb,bool isShow,int sort)
        {
            Name = name;
            Thumb = thumb;
            IsShow = isShow;
            Sort = sort;
        }
    }
}
