using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Comments
{
    /// <summary>
    /// 评论信息
    /// </summary>
    [Serializable]
    public class CommentInfo
    {
        public Guid GoodsId { get; private set; }
        public Guid AuthorId { get; private set; }
        public string Body { get; private set; }
        public  IList<string> Thumbs { get; private set; }

        public CommentInfo(Guid goodsId,Guid authorId,string body,IList<string> thumbs)
        {
            GoodsId = goodsId;
            AuthorId = authorId;
            Body = body;
            Thumbs = thumbs;
        }
    }
}
