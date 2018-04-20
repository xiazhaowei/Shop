using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Categorys
{
    public class UpdateCategoryCommand : Command<Guid>
    {
        public string Name { get; private set; }
        public string Url { get; private set; }
        public string Thumb { get; private set; }
        public CategoryType Type { get; private set; }
        public bool IsShow { get; private set; }
        public int Sort { get;private set; }

        public UpdateCategoryCommand() { }
        public UpdateCategoryCommand(
            string name,
            string url,string thumb,
            CategoryType type,
            bool isShow,
            int sort)
        {
            Name = name;
            Url = url;
            Thumb = thumb;
            Type = type;
            IsShow = isShow;
            Sort = sort;
        }
    }
}
