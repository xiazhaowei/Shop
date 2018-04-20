using ENode.Commanding;
using System;
using System.Collections.Generic;

namespace Shop.Commands.Goodses
{
    public class UpdateParamsCommand:Command<Guid>
    {
        public IList<GoodsParamInfo> GoodsParams { get;private set; }

        public UpdateParamsCommand() { }
        public UpdateParamsCommand(Guid goodsId,IList<GoodsParamInfo> goodsParams) : base(goodsId)
        {
            GoodsParams = goodsParams;
        }
    }
}
