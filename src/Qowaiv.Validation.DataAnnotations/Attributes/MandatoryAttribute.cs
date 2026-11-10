using Qowaiv.Reflection;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Specifies that a field is mandatory (for value types the default value is not allowed).</summary>
[AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
[Validates(typeof(object))]
public sealed class MandatoryAttribute : RequiredAttribute
{
    /// <summary>Initializes a new instance of the <see cref="MandatoryAttribute"/> class.</summary>
    public MandatoryAttribute()
    {
        ErrorMessageResourceType = typeof(QowaivValidationMessages);
        ErrorMessageResourceName = nameof(QowaivValidationMessages.MandatoryAttribute_ValidationError);
    }

    /// <summary>Gets or sets a value that indicates whether an empty string is allowed.</summary>
    public bool AllowUnknownValue { get; set; }

    /// <inheritdoc />
    public override bool RequiresValidationContext => true;

    /// <inheritdoc />
    [Pure]
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        => this.PostValidate(Check(value, validationContext))
        ? null
        : ValidationMessage.Error(FormatErrorMessage(validationContext.DisplayName), validationContext.MemberNames());

    /// <summary>Returns true if the value is not null and value types are
    /// not equal to their default value, otherwise false.
    /// </summary>
    /// <remarks>
    /// The unknown value is expected to be static field or property of the type with the name "Unknown".
    /// </remarks>
    [Pure]
    public override bool IsValid(object? value)
        => this.PostValidate(Check(value, null));

    [Pure]
    private bool Check(object? value, ValidationContext? ctx)
    {
        // Null and string are handled the same as by RequiredAttribute.
        if (value is null) return false;
        else if (value is string s) return s.Length > 0 || AllowUnknownValue;
        else
        {
            var type = value.GetType();
            return !IsDefault(type) && !IsUknown(type);
        }

        bool IsDefault(Type type)
            => !IsNullable(type, ctx)
            && value.Equals(SvoValues.Default(type));

        bool IsUknown(Type type)
            => !AllowUnknownValue
            && value.Equals(SvoValues.Unknown(type));
    }

    [Pure]
    private static bool IsNullable(Type type, ValidationContext? ctx)
    {
        // Reference types are nullable by design.
        if (!type.IsValueType) return true;

        if (Member.New(ctx) is { } member)
        {
            if (!Members.TryGetValue(member, out var nullable))
            {
                nullable = member.IsNullable();
                Members[member] = nullable;
                return nullable;
            }
            else return nullable;
        }
        else return false;
    }

    private readonly record struct Member(Type Declaring, string Name)
    {
        [Pure]
        public bool IsNullable()
            => Name is { Length: > 0 }
            && (Declaring.GetProperty(Name)?.PropertyType ?? Declaring.GetField(Name)?.FieldType) is
            {
                IsValueType: true,
                IsNullable: true,
            };

        [Pure]
        public static Member? New(ValidationContext? ctx)
            => ctx is { ObjectType: { } declaring, MemberName: { Length: > 0 } name }
            ? new(declaring, name)
            : null;
    }

    private static readonly ConcurrentDictionary<Member, bool> Members = [];
}
