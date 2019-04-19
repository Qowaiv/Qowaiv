using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.ComponentModel.Rules;
using Qowaiv.DomainModel;
using Qowaiv.DomainModel.EventSourcing;
using Qowaiv.Financial;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.Xample.Domain
{
    public class Order : EventSourcedAggregateRoot<Order>
    {
        public Order()
        {
            OrderItems = new ChildCollection<OrderItem>(Tracker);
        }
        public ChildCollection<OrderItem> OrderItems { get; }

        [DefinedEnumValuesOnly]
        public OrderStatus Status
        {
            get => GetProperty<OrderStatus>();
            private set => SetProperty(value);
        }

        [Display(Name = "Order address")]
        public Address OrderAddress
        {
            get => GetProperty<Address>();
            private set => SetProperty(value);
        }

        [Display(Name = "Total price")]
        public Money TotalPrice
        {
            get
            {
                var total = (OrderItems.FirstOrDefault()?.Price).GetValueOrDefault();
                for (var i = 1; i < OrderItems.Count; i++)
                {
                    total += OrderItems[i].Price;
                }
                return total;
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return Rule.ConditionalRequired(validationContext, Status == OrderStatus.Shipable, nameof(OrderAddress));
        }

        public Result<Order> AddOrderItem(Commands.AddOrderItem cmd)
        {
            var e = new Events.AddOrderItem
            {
                OrderId = Id,
                Price = cmd.Price,
                ProductId = cmd.ProductId,
            };
            return ApplyChange(e);
        }
        internal void Apply(Events.AddOrderItem e)
        {
            var order = new OrderItem(Tracker)
            {
                Price = e.Price,
                ProductId = e.ProductId,
            };
            OrderItems.Add(order);
        }
    }
}
