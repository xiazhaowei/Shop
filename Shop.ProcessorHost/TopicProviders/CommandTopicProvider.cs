using ECommon.Components;
using ENode.Commanding;
using ENode.EQueue;
using Shop.Common;

namespace Shop.ProcessorHost.TopicProviders
{
    [Component]
    public class CommandTopicProvider : AbstractTopicProvider<ICommand>
    {
        public override string GetTopic(ICommand command)
        {
            return Topics.ShopCommandTopic;
        }
    }
}
