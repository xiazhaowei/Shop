using Shop.QueryServices.Dtos;
using System;

namespace Shop.QueryServices
{
    public interface IPaymentQueryService
    {
        Payment FindPayment(Guid paymentId);
    }
}