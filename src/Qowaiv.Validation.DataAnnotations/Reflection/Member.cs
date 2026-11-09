using System.Linq.Expressions;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace Qowaiv.Validation.DataAnnotations.Reflection;

internal abstract class Member
{
    /// <inheritdoc cref="MemberInfo.Name" />
    public abstract string Name { get; }

    /// <summary>Gets the type of the member.</summary>
    public abstract Type MemberType { get; }

    /// <summary>Indicates if the member can be read.</summary>
    public abstract bool CanRead { get; }

    /// <inheritdoc cref="MemberInfo.GetCustomAttributes(bool)" />
    [Pure]
    public abstract object[] GetCustomAttributes();

    /// <summary>Gets the compiled accessor of the member.</summary>
    public abstract Func<object, object?> Get { get; }

    /// <summary>Indicates that the members is indexed.</summary>
    public abstract bool IsNotIndexed { get; }

    /// <summary>Members that can safely be accessed to not need a try catch.</summary>
    public abstract bool IsSafe { get; }

    /// <summary>
    /// Get the <see cref="RequiredMemberAttribute"/> attribute instance for reference types
    /// that are not nullable and defined as required properties.
    /// </summary>
    [Pure]
    public RequiredAttribute? GetRequiredMemberAttribute()
    {
        if (MemberType.IsNonNullableValueType()) return null;

        var attributes = GetCustomAttributes().Select(a => a.GetType());

        return attributes.Any(a => a.FullName == "System.Runtime.CompilerServices.RequiredMemberAttribute")
            && !attributes.Any(a => a.FullName == "System.Runtime.CompilerServices.NullableAttribute")
            ? RequiredMember
            : null;
    }

    private static readonly RequiredMemberAttribute RequiredMember = new();

    /// <summary>Gets a field member.</summary>
    [Pure]
    public static Member New(FieldInfo info) => new Field(info);

    /// <summary>Gets a property member.</summary>
    [Pure]
    public static Member New(PropertyInfo prop) => new Property(prop);

    private sealed class Field(FieldInfo info) : Member
    {
        private readonly FieldInfo Info = info;

        public override string Name => Info.Name;

        public override Type MemberType => Info.FieldType;

        public override bool CanRead => true;

        public override bool IsSafe => true;

        [Pure]
        public override object[] GetCustomAttributes() => Info.GetCustomAttributes(inherit: true);

        [Pure]
        public override Func<object, object?> Get => field ??= Compile();

        [Pure]
        public override bool IsNotIndexed => true;

        [Pure]
        private Func<object, object?> Compile()
        {
            var obj = Parameter(typeof(object), "obj");
            var body = Convert(Expression.Field(Convert(obj, Info.DeclaringType!), Info), typeof(object));
            return Lambda<Func<object, object?>>(body, obj).Compile();
        }
    }

    private sealed class Property(PropertyInfo prop) : Member
    {
        private readonly PropertyInfo Info = prop;

        public override string Name => Info.Name;

        public override Type MemberType => Info.PropertyType;

        public override bool CanRead => Info.CanRead;

        public override bool IsSafe { get; } = prop.IsSimpleFieldAccessor;

        [Pure]
        public override object[] GetCustomAttributes() => Info.GetCustomAttributes(inherit: true);

        [Pure]
        public override Func<object, object?> Get => Accessor ??= Compile();

        [Pure]
        public override bool IsNotIndexed => Info.GetIndexParameters() is not { Length: > 0 };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<object, object?>? Accessor;

        [Pure]
        private Func<object, object?> Compile()
        {
            var obj = Parameter(typeof(object), "obj");
            var body = Convert(
                Expression.Property(Convert(obj, Info.DeclaringType!), Info),
                typeof(object));
            return Lambda<Func<object, object?>>(body, obj).Compile();
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    [Validates(typeof(object))]
    private sealed class RequiredMemberAttribute : RequiredAttribute
    {
        [Pure]
        public override bool IsValid(object? value) => value is not null;
    }
}
