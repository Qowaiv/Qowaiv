using Qowaiv.Financial;
using System;

namespace Qowaiv.Xample.Domain.Events
{
    public class AddOrderItem
    {
        public Guid OrderId { get; set; }
        public Money Price { get; set; }
        public Guid ProductId { get; set; }
    }
}
