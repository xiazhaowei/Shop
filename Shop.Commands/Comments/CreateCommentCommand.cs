using ENode.Commanding;
using System;
using System.Collections.Generic;

namespace Shop.Commands.Comments
{
    public class CreateCommentCommand:Command<Guid>
    {
        public Guid GoodsId { get; private set; }
        public Guid AuthorId { get; private set; }
        public string Body { get; private set; }
        
        public IList<string> Thumbs { get; private set; }

        public Single PriceRate { get; private set; }
        public Single DescribeRate { get; private set; }
        public Single QualityRate { get; private set; }
        public Single ExpressRate { get; private set; }

        public CreateCommentCommand() { }
        public CreateCommentCommand(
            Guid id,
            Guid goodsId,
            Guid authorId,
            string body,
            IList<string> thumbs,
            Single priceRate,
            Single describeRate,
            Single qualityRate,
            Single expressRate):base(id)
        {
            GoodsId = goodsId;
            AuthorId = authorId;
            Body = body;
            Thumbs = thumbs;
            PriceRate = priceRate;
            DescribeRate = describeRate;
            QualityRate = qualityRate;
            ExpressRate = expressRate;
        }
    }
}
