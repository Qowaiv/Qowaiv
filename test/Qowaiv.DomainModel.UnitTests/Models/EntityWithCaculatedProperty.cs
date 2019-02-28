namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EntityWithCaculatedProperty : Entity<int>
    {
        public EntityWithCaculatedProperty(int id) : base(id)
        {
        }

        public decimal Value
        {
            get => GetProperty<decimal>();
            set => SetProperty(value);
        }
        public int Repertitions
        {
            get => GetProperty<int>();
            set => SetProperty(value);
        }

        public decimal Total => Value * Repertitions;
    }
}
