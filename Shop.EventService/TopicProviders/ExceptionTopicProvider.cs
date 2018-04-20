using ECommon.Components;
using ENode.EQueue;
using ENode.Infrastructure;
using Shop.Common;

namespace Shop.EventService.TopicProviders
{
    [Component]
    public class ExceptionTopicProvider : AbstractTopicProvider<IPublishableException>
    {
        public override string GetTopic(IPublishableException exception)
        {
            return Topics.ShopExceptionTopic;
        }
    }
}
