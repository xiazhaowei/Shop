using ECommon.Components;
using ENode.Commanding;
using ENode.EQueue;
using Shop.Common;

namespace Shop.EventService.TopicProviders
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
