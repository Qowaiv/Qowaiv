namespace Qowaiv.Validation.DataAnnotations;

internal static class Validate
{
    public static void Model(Nested nested, ValidateContext ctx)
    {
        // instance has not been validated yet.
        if (!ctx.Done.Add(nested.Instance)) return;

        (var instance, var annotations, var path) = nested;

        var context = ctx.Validation(nested);

        foreach (var member in annotations.Members)
        {
            Member(member, nested, ctx, context);
        }

        if (annotations.CheckEnumerable && instance is IEnumerable enumerable)
        {
            Enumerable(enumerable, path, ctx);
        }

        if (annotations.CheckValidatable && instance is IValidatableObject validatable)
        {
            // Reset validation context
            context.MemberName = null;
            context.DisplayName = context.ObjectType.Name;

            ctx.AddMessages(validatable.Validate(context), path);
        }
    }

    /// <summary>Validates the items of the enumerable.</summary>
    private static void Enumerable(IEnumerable enumerable, MemberPath path, ValidateContext ctx)
    {
        var enumTyped = enumerable.GetType().GetEnumerableType() is { } enumerated
            ? TypeAnnotations.Get(enumerated)
            : null;

        // We have to reset it if the type is not sealed.
        if (enumTyped?.CheckInheritance is true) enumTyped = null;

        var index = -1;

        // As enumerable types are often homogeneous, the annotations of the
        // previously validated item are cached to avoid a lookup per item.
        Type? lastType = null;
        TypeAnnotations? lastTyped = null;

        foreach (var item in enumerable)
        {
            index++;
            if (item is null) continue;

            TypeAnnotations? typed = enumTyped;

            if (typed is null)
            {
                var type = item.GetType();

                if (type == lastType)
                {
                    typed = lastTyped;
                }
                else
                {
                    lastType = type;
                    typed = lastTyped = TypeAnnotations.Get(type);
                }
            }

            if (typed is not null)
            {
                Model(new Nested(item, typed, path.Child(index)), ctx);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Member(MemberAnnotations member, Nested nested, ValidateContext ctx, ValidationContext context)
    {
        object? value;

        if (member.IsSafe)
        {
            value = member.GetValue(nested.Instance);
        }
        else
        {
            try { value = member.GetValue(nested.Instance); }
            catch
            {
                ctx.AddMessage(ValidationMessage.Error($"The value is inaccessible.", member.Name), nested.Path);
                return;
            }
        }

        context.MemberName = member.Name;
        context.DisplayName = member.Display?.GetName() ?? member.Name;

#pragma warning disable S3267 // Do not use LINQ: it allocates on this hot path.
        // Stop on first required failure and do not validate further.
        foreach (var attr in member.Attributes)
        {
            if (ctx.AddMessage(attr.GetValidationMessage(value, context), nested.Path) && attr is RequiredAttribute)
            {
                return;
            }
        }
#pragma warning restore S3267

        if (value is { } && RecursionOf(member, value) is { } typed)
        {
            Model(new Nested(value, typed, nested.Path.Child(member.Name)), ctx);
        }
    }

    /// <summary>Gets the annotations to recurse into, or null if no recursion is applicable.</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TypeAnnotations? RecursionOf(MemberAnnotations member, object value)
    {
        // For a member whose runtime type is guaranteed to equal the declared
        // type, the annotations are known upfront, avoiding a lookup. Cyclic
        // references lack upfront annotations and are resolved via the store.
        var annotations = member is { FixedType: true, Type: { } declared }
            ? declared
            : TypeAnnotations.Get(value.GetType());

        return annotations is { CheckRecursive: true } type ? type : null;
    }
}
