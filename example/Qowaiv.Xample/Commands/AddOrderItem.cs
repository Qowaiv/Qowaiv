using Qowaiv.Financial;
using System;

namespace Qowaiv.Xample.Commands
{
    public class AddOrderItem
    {
        public Money Price { get; set; }
        public Guid ProductId { get; set; }
    }
}
