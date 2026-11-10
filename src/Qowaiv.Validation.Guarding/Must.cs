namespace Qowaiv.Validation.Guarding;

/// <summary>A monad to allow declarative guarding conditions on a subject.</summary>
/// <typeparam name="TSubject">
/// The type of the subject.
/// </typeparam>
public sealed class Must<TSubject> where TSubject : class
{
    /// <summary>Initializes a new instance of the <see cref="Must{TSubject}"/> class.</summary>
    internal Must(TSubject subject) => Subject = Guard.NotNull(subject);

    /// <summary>Gets the subject to guard.</summary>
    public TSubject Subject { get; }

    /// <summary>Guards the condition to be true; returns a
    /// <see cref="Result{TSubject}"/> with the specified message(s)
    /// otherwise.
    /// </summary>
    /// <param name="condition">
    /// The guarding condition.
    /// </param>
    /// <param name="messages">
    /// The message(s) to communicate the failed guard.
    /// </param>
    [Pure]
    public Result<TSubject> Be(bool condition, params IValidationMessage[] messages)
        => condition ? Subject : Result.For(Subject, messages);

    /// <summary>Guards the condition to be true; returns the
    /// error message otherwise.
    /// </summary>
    /// <param name="condition">
    /// The guarding condition.
    /// </param>
    /// <param name="message">
    /// The message to communicate the failed guard.
    /// </param>
    /// <param name="args">
    /// Optional arguments to format the message with.
    /// </param>
    [Pure]
    public Result<TSubject> Be(bool condition, string message, params object[] args)
        => Be(condition, ValidationMessage.Error(string.Format(message, args)));

    /// <summary>Guards the condition to be false; returns the
    /// error message otherwise.
    /// </summary>
    /// <param name="condition">
    /// The guarding condition.
    /// </param>
    /// <param name="message">
    /// The message to communicate the failed guard.
    /// </param>
    /// <param name="args">
    /// Optional arguments to format the message with.
    /// </param>
    [Pure]
    public Result<TSubject> NotBe(bool condition, string message, params object[] args)
        => Be(!condition, message, args);

    /// <summary>Guards the entity to exist for the specified id; returns the
    /// error message otherwise.
    /// </summary>
    /// <param name="id">
    /// The id of the entity.
    /// </param>
    /// <param name="selector">
    /// The selector that tries to find the entity based on the subject.
    /// </param>
    /// <param name="message">
    /// The message to communicate the failed guard.
    /// </param>
    [Pure]
    public Result<TSubject> Exist<TId, TEntity>(TId id, Func<TSubject, TId, TEntity?> selector, IValidationMessage? message)
        => Be(Guard.NotNull(selector).Invoke(Subject, id) is { }, message ?? EntityNotFound.ForId(id!));

    /// <summary>Guards the entity to exist for the specified id; returns the
    /// error message otherwise.
    /// </summary>
    /// <param name="id">
    /// The id of the entity.
    /// </param>
    /// <param name="selector">
    /// The selector that tries to find the entity based on the subject.
    /// </param>
    [Pure]
    public Result<TSubject> Exist<TId, TEntity>(TId id, Func<TSubject, TId, TEntity?> selector)
        where TEntity : class
        => Exist(id, selector, null);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"Must<{typeof(TSubject)}>";
}
