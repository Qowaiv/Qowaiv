﻿namespace Qowaiv.Financial;

/// <summary>When currencies differ, operations on money are not possible.</summary>
[Serializable]
public class CurrencyMismatchException : Exception
{
    /// <summary>Initializes a new instance of the <see cref="CurrencyMismatchException"/> class.</summary>
    public CurrencyMismatchException() { }

    /// <summary>Initializes a new instance of the <see cref="CurrencyMismatchException"/> class.</summary>
    public CurrencyMismatchException(string message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="CurrencyMismatchException"/> class.</summary>
    public CurrencyMismatchException(string message, Exception inner) : base(message, inner) { }

    /// <summary>Initializes a new instance of the <see cref="CurrencyMismatchException"/> class.</summary>
    public CurrencyMismatchException(Currency cur0, Currency cur1, string operation)
        : this(string.Format(QowaivMessages.CurrencyMismatchException, operation, cur0, cur1)) { }

    /// <summary>Initializes a new instance of the <see cref="CurrencyMismatchException"/> class.</summary>
    protected CurrencyMismatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
