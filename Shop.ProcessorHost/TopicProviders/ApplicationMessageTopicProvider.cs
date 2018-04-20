using Shop.Common;
using ECommon.Components;
using ENode.EQueue;
using ENode.Infrastructure;

namespace Shop.ProcessorHost.TopicProviders
{
    [Component]
    public class ApplicationMessageTopicProvider : AbstractTopicProvider<IApplicationMessage>
    {
        public override string GetTopic(IApplicationMessage applicationMessage)
        {
            return Topics.ShopApplicationMessageTopic;
        }
    }
}
