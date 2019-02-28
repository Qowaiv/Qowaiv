using Qowaiv.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EntityWithInitScope : Entity<int>
    {
        public EntityWithInitScope() { }
        public EntityWithInitScope(int id) : base(id) { }

        [Mandatory]
        public string Name
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        [Mandatory]
        public Date StartDate
        {
            get => GetProperty<Date>();
            set => SetProperty(value);
        }

        public static EntityWithInitScope FromData(int id, string name, Date startDate)
        {
            var entity = new EntityWithInitScope(id);

            entity.SetProperties(() =>
            {
                entity.Name = name;
                entity.StartDate = startDate;
            });

            return entity;
        }
    }
}
