namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validates if all items of the collection are valid according to the wrapped validator.</summary>
/// <typeparam name="TValidator">
/// The validator to validate the items with.
/// </typeparam>
[AttributeUsage(AttributeTarget.Member, AllowMultiple = true, Inherited = false)]
[CLSCompliant(false)]
[Validates(typeof(IEnumerable))]
public class ItemsAttribute<TValidator> : ValidationAttribute
    where TValidator : ValidationAttribute
{
    /// <summary>Initializes a new instance of the <see cref="ItemsAttribute{TValidator}"/> class.</summary>
    /// <param name="args">
    /// The args to provide to the constructor of <typeparamref name="TValidator"/> .
    /// </param>
    public ItemsAttribute(params object[] args) : this(Create(args)) { }

    /// <summary>Initializes a new instance of the <see cref="ItemsAttribute{TValidator}"/> class.</summary>
    /// <param name="validator">
    /// The validator to use.
    /// </param>
    protected ItemsAttribute(TValidator validator) => Validator = Guard.NotNull(validator);

    private readonly TValidator Validator;

    /// <inheritdoc />
    public override bool RequiresValidationContext => true;

    /// <inheritdoc />
    [Pure]
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var index = 0;

        if (value is IEnumerable enumerable)
        {
            var memberNames = new MemberNames(validationContext.MemberName!);

            foreach (var item in enumerable)
            {
                if (!Validator.IsValid(item))
                {
                    memberNames.AddIndex(index);
                }
                index++;
            }

            if (memberNames.Count > 0)
            {
                return ValidationMessage.Error(FormatErrorMessage(validationContext.DisplayName), memberNames);
            }
        }
        return null;
    }

    /// <inheritdoc />
    [Pure]
    public override string FormatErrorMessage(string name)
    {
        if (Init)
        {
            lock (Locker)
            {
                if (Init)
                {
                    if (ErrorMessageResourceName is { }) Validator.ErrorMessageResourceName = ErrorMessageResourceName;
                    if (ErrorMessageResourceType is { }) Validator.ErrorMessageResourceType = ErrorMessageResourceType;

                    // If the error message is updated, reset the resource based setting.
                    if (ErrorMessage is { })
                    {
                        Validator.ErrorMessageResourceName = null;
                        Validator.ErrorMessageResourceType = null;
                        Validator.ErrorMessage = ErrorMessage;
                    }
                    Init = false;
                }
            }
        }
        return Validator.FormatErrorMessage(name);
    }

    private readonly Lock Locker = new();

    private volatile bool Init = true;

    [Pure]
    private static TValidator Create(object[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                return Activator.CreateInstance<TValidator>();
            }

            // if there are multiple ctors with arguments.
            var ctor = typeof(TValidator).GetConstructors().Single(c => c.GetParameters().Length > 0);
            var input = new List<object>();
            var i = 0;

            foreach (var parameter in ctor.GetParameters())
            {
                // This should be the last argument: what is left is put in.
                if (parameter.ParameterType.IsArray)
                {
                    input.Add(args.Skip(i).ToArray());
                }
                else
                {
                    input.Add(args[i++]);
                }
            }
            return (TValidator)ctor.Invoke([.. input]);
        }
        catch (Exception x)
        {
            throw new ArgumentException("The provided args could not be used to create the validator.", nameof(args), x);
        }
    }
}
