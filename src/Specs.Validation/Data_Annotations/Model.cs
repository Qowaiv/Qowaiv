#pragma warning disable

using Qowaiv.Validation.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.IO;

internal static class Model
{
    public static class ClockDependent
    {
        public sealed class InFuture
        {
            [InFuture]
            public DateOnly? ExpiryDate { get; init; }
        }

        public sealed class InPast
        {
            [InPast]
            public Year YearOfConstruction { get; init; }
        }

        public sealed class NotInFuture
        {
            [NotInFuture]
            public DateOnly DateOfBirth { get; init; }
        }

        public sealed class NotInPast
        {
            [NotInPast]
            public DateOnly ExpiryDate { get; init; }
        }
    }

    public static class With
    {
        public sealed class AnnotatedProperty
        {
            [Length.AtLeast(3)]
            public string? Name { get; init; }
        }

        public sealed class Display
        {
            [Length.AtMost(2)]
            [Display(Name = "Property")]
            public string? Prop { get; init; }
        }

        public sealed class InaccessibleProperty
        {
            [Allowed<int>("42")]
            public int ThrowsOnGet => throw new NotImplementedException(ToString());
        }

        public sealed class IndexedProperty
        {
            [Required]
            public int this[int index] => index * 42;
        }

        public sealed class InheritableMember
        {
            public Inheritable? Member { get; init; }
        }

        public sealed class Fields
        {
            [Allowed<int>(42)]
            public readonly int Answer = 007;

            [Mandatory]
            public static string NonInstance = string.Empty;

            [Allowed<int>(42)]
            private int Private;
        }

        public sealed class RequiredOnValueType
        {
            [Required]
            public int? Required { get; init; }

            [Required]
            public bool Ignored { get; init; }

            [Mandatory]
            public EmailAddress Mandatory { get; init; }
        }

        public sealed class SetOnlyProperty
        {
            // This is a test to check if write-only properties are handled correctly.
            [Mandatory]
            public int SomeProperty
            {
                set => field = value;
            }
        }

        public class SelfReference
        {
            [Allowed<int>("42")]
            public int Value { get; init; }

            public SelfReference? Next { get; set; }
        }

        public sealed class MutualA
        {
            public MutualB? B { get; init; }
        }

        public class MutualB
        {
            public MutualA? A { get; init; }

            [Allowed<int>("42")]
            public int Value { get; init; }
        }

        public sealed class SealedCycle
        {
            public UnsealedInner? Inner { get; init; }
        }

        public class UnsealedInner
        {
            public SealedCycle? Cycle { get; init; }

            [Allowed<int>("42")]
            public int Value { get; init; }
        }
    
        public class NonGenericEnumerable
        {
            [Any]
            public ArrayList Items { get; init; } = new();
        }

        public class Enumerables
        {
            [Collection.AtLeast(4)]
            public IEnumerable Items { get; init; } = new Childeren();

            public class Child
            {
                [Mandatory]
                public int Mandatory { get; init; } = 42;
            }

            public class Childeren : IEnumerable
            {
                public IEnumerator GetEnumerator()
                {
                    yield return new Child();
                    yield return new Child();
                    yield return new Child();
                    yield return new Child();
                }
            }
        }
    }

    public static class Without
    {
        public sealed class Annotations
        {
            public FileInfo? File { get; init; }

            public string? Name { get; init; }

            public int Number { get; init; }

            public DateTime CreatedUtc => File?.CreationTimeUtc ?? Clock.UtcNow();

            [SkipValidation]
            public required string Required { get; init; }

            public Parent? WithLoop { get; init; }

            public sealed class Parent
            {
                public Child[] Childen { get; init; } = [];
            }

            public sealed class Child
            {
                public Parent? Parent { get; init; }
            }
        }
    }

    public class Inheritable { }

    public class Inherited : Inheritable
    {
        [Allowed<int>("42")]
        public int Value { get; init; }
    }
}
