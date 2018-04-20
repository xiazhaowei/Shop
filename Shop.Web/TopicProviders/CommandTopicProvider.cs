using ECommon.Components;
using ENode.Commanding;
using ENode.EQueue;
using Shop.Common;

namespace Shop.Web.TopicProviders
{
    [Component]
    public class CommandTopicProvider : AbstractTopicProvider<ICommand>
    {
        public override string GetTopic(ICommand command)
        {
            //如果是付款边界命令走付款Topic
            //if (command is CreatePaymentCommand || command is CompletePaymentCommand || command is CancelPaymentCommand)
            //{
            //    return Topics.PaymentCommandTopic;
            //}
            ////如果是订单边界命令走 购物Topic
            //if (command is PlaceOrderCommand ||
            //    command is CloseOrderCommand ||
            //    command is ConfirmOneReservationCommand ||
            //    command is ConfirmPaymentCommand ||
            //    command is MarkAsSuccessCommand)
            //{
            //    return Topics.BuyCommandTopic;
            //}
            //其余走商城Topic
            return Topics.ShopCommandTopic;
        }
    }
}
