using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Rules;
using Qowaiv.DomainModel;
using Qowaiv.DomainModel.EventSourcing;
using Qowaiv.FirstPopCoffeeCompany.RoastPlanning.Events;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.FirstPopCoffeeCompany.RoastPlanning
{
    public sealed class RoastSchedule : EventSourcedAggregateRoot<RoastSchedule>
    {
        public RoastSchedule() { }

        [Mandatory]
        public Date StartDate
        {
            get => GetProperty<Date>();
            private set => SetProperty(value);
        }

        [Mandatory]
        public Date EndDate
        {
            get => GetProperty<Date>();
            private set => SetProperty(value);
        }

        public ChildCollection<Date> RoastDays { get; } = new ChildCollection<Date>();

        public Result<RoastSchedule> SelectRoastDays(Date[] roastDays)
        {
            return ApplyChange(new SelectRoastDays(roastDays));
        }
        internal void Apply(SelectRoastDays roastDays)
        {
            Clear(RoastDays);
            AddRange(RoastDays, roastDays.Selected);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.NotDefault() && EndDate.NotDefault() && EndDate < StartDate)
            {
                yield return ValidationMessage.Error("The end date can not be before the start date", nameof(EndDate));
            }

            if (RoastDays.Any(day => day < StartDate || day > EndDate))
            {
                yield return ValidationMessage.Error("No roast day should be before the start or after the end date.", nameof(RoastDays));
            }
            if (RoastDays.Distinct().Count() != RoastDays.Count)
            {
                yield return ValidationMessage.Error("Roast days can only be selected once.", nameof(RoastDays));
            }
        }

        public static Result<RoastSchedule> Create(CreateRoastSchedule command)
        {
            var aggregate = new RoastSchedule();
            return aggregate.ApplyChange(command);
        }

        internal void Apply(CreateRoastSchedule @event)
        {
            Id = @event.Id;
            StartDate = @event.StartDate;
            EndDate = @event.EndDate;
        }
    }
}
