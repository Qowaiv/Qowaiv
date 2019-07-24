namespace Qowaiv.Genealogy.Commands
{
    public class CreatePerson
    {
        public Gender Gender { get; set; }

        public string PersonalName { get; set; }

        public string FamilyName { get; set; }

        public Date DateOfBirth { get; set; }

        public Date? DateOfDeath { get; set; }

        public EmailAddress Email { get; set; }
    }
}
