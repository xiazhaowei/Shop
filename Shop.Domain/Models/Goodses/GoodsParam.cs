namespace Shop.Domain.Models.Goodses
{
    /// <summary>
    /// 商品参数
    /// </summary>
    public class GoodsParam
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public GoodsParam(string name,string value)
        {
            Name = name;
            Value = value;
        }
    }
}
