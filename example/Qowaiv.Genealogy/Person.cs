using Qowaiv.DomainModel.EventSourcing;
using Qowaiv.Genealogy.Commands;
using Qowaiv.Genealogy.Events;
using Qowaiv.Genealogy.Mapping;
using Qowaiv.Validation.Abstractions;

namespace Qowaiv.Genealogy
{
    public class Person : EventSourcedAggregateRoot<Person>
    {
        private static readonly PersonValidator _validator = new PersonValidator();

        public Person() : base(_validator) { }

        public Gender Gender
        {
            get => GetProperty<Gender>();
            private set => SetProperty(value);
        }

        public string PersonalName
        {
            get => GetProperty<string>();
            private set => SetProperty(value);
        }

        public string FamilyName
        {
            get => GetProperty<string>();
            private set => SetProperty(value);
        }

        public Date DateOfBirth
        {
            get => GetProperty<Date>();
            private set => SetProperty(value);
        }

        public Date? DateOfDeath
        {
            get => GetProperty<Date?>();
            private set => SetProperty(value);
        }

        public EmailAddress Email
        {
            get => GetProperty<EmailAddress>();
            private set => SetProperty(value);
        }

        public Result<Person> Update(UpdatePerson cmd)
        {
            Guard.NotNull(cmd, nameof(cmd));

            return ApplyChange(cmd.Map<PersonUpdated>());
        }

        internal void Apply(PersonUpdated e)
        {
            Gender = e.Gender;
            PersonalName = e.PersonalName;
            FamilyName = e.FamilyName;
            DateOfBirth = e.DateOfBirth;
            DateOfDeath = e.DateOfDeath;
            Email = e.Email;
        }

        public static Result<Person> Create(CreatePerson cmd)
        {
            Guard.NotNull(cmd, nameof(cmd));

            var person = new Person();
            var updated = cmd.Map<PersonUpdated>();
            return person.ApplyChanges(new PersonCreated(), updated);
        }
    }
}
