using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Request.Goodses
{
    public class CommentRequest
    {
        public Guid GoodsId { get; set; }
        public string Body { get; set; }
        public IList<string> Thumbs { get; set; }

        public float PriceRate { get; set; }
        public float DescribeRate { get; set; }
        public float QualityRate { get; set; }
        public float ExpressRate { get; set; }
    }
}