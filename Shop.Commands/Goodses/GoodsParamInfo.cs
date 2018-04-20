using System;

namespace Shop.Commands.Goodses
{
    [Serializable]
    public class GoodsParamInfo
    {
        public string Name { get;private  set; }
        public string Value { get; private set; }

        public GoodsParamInfo(string name,string value)
        {
            Name = name;
            Value = value;
        }
    }
}
