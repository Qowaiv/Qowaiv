using System;
using Qowaiv.DomainModel;
using Qowaiv.DomainModel.Tracking;
using Qowaiv.Financial;

namespace Qowaiv.Xample.Domain
{
    public class OrderItem : Entity<OrderItem>
    {
        internal OrderItem(ChangeTracker tracker) : base(tracker)
        {
        }
        public Guid ProductId
        {
            get => GetProperty<Guid>();
            internal set => SetProperty(value);
        }
        public Money Price
        {
            get => GetProperty<Money>();
            internal set => SetProperty(value);
        }
    }
}
