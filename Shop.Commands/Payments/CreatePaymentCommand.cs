using System;
using System.Collections.Generic;
using ENode.Commanding;

namespace Shop.Commands.Payments
{
    public class CreatePaymentCommand : Command<Guid>
    {
        public Guid OrderId { get;private set; }
        public string Description { get;private set; }
        public decimal TotalAmount { get;private set; }
        public IEnumerable<PaymentLine> Lines { get;private set; }

        public CreatePaymentCommand() { }
        public CreatePaymentCommand(Guid id,
            Guid orderId,
            string description,
            decimal totalAmount,
            IEnumerable<PaymentLine> lines):base(id)
        {
            OrderId = orderId;
            Description = description;
            TotalAmount = totalAmount;
            Lines = lines;
        }
    }
}
