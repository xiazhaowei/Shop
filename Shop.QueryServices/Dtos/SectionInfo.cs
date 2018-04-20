using System;

namespace Shop.QueryServices.Dtos
{
    /// <summary>
    /// 表示一个行业的信息
    /// </summary>
    public class SectionInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SectionInfo()
        {
            Id = Guid.Empty;
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
