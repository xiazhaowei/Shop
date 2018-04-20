using ECommon.Components;
using ENode.Commanding;
using ENode.EQueue;
using Shop.Common;

namespace Shop.Api.TopicProviders
{
    [Component]
    public class CommandTopicProvider : AbstractTopicProvider<ICommand>
    {
        public override string GetTopic(ICommand command)
        {
            //其余走商城Topic
            return Topics.ShopCommandTopic;
        }
    }
}
