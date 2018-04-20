using Shop.Common;
using ECommon.Components;
using ENode.EQueue;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Shop.ProcessorHost.TopicProviders
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
