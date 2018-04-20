using ENode.Eventing;
using Shop.Domain.Models.Grantees;
using System;

namespace Shop.Domain.Events.Grantees
{
    /// <summary>
    /// 后台添加资料认证
    /// </summary>
    [Serializable]
    public class AddVerificationEvent:DomainEvent<Guid>
    {
        public Verification Verification { get; private set; }

        public AddVerificationEvent() { }
        public AddVerificationEvent(Verification verification)
        {
            Verification = verification;
        }
    }
}
