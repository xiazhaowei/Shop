using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Categorys
{
    public class CreateCategoryCommand : Command<Guid>
    {
        public Guid ParentId { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
        public string Thumb { get; private set; }
        public CategoryType Type { get; private set; }
        public bool IsShow { get; private set; }
        public int Sort { get; private set; }

        public CreateCategoryCommand() { }
        public CreateCategoryCommand(Guid id,
            Guid parentId,
            string name,
            string url,
            string thumb,
            CategoryType type,
            bool isShow,
            int sort):base(id)
        {
            ParentId = parentId;
            Name = name;
            Url = url;
            Thumb = thumb;
            Type = type;
            IsShow = isShow;
            Sort = sort;
        }
    }
}
