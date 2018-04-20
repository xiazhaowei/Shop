using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.Comments;
using System.Threading.Tasks;
using Xia.Common.Extensions;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class CommentViewModelGenerator:BaseGenerator,
        IMessageHandler<CommentCreatedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(CommentCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    GoodsId = evnt.Info.GoodsId,
                    UserId = evnt.Info.AuthorId,
                    Body = evnt.Info.Body,
                    CreatedOn = evnt.Timestamp,
                    Rate = evnt.RateInfo.Rate,
                    PriceRate = evnt.RateInfo.PriceRate,
                    DescribeRate = evnt.RateInfo.DescribeRate,
                    QualityRate = evnt.RateInfo.QualityRate,
                    ExpressRate = evnt.RateInfo.ExpressRate,
                    Thumbs=evnt.Info.Thumbs.ExpandAndToString("|"),
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.GoodsCommentsTable);
            });
        }
        
    }
}
