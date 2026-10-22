using Qowaiv.Financial;
using Qowaiv.IO;
using Qowaiv.Statistics;

namespace Qowaiv.Linq;

/// <summary>Extensions on <see cref="IEnumerable{T}" />.</summary>
public static class QowaivEnumerable
{
    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the average of a sequence of <see cref="Amount" /> values that are obtained
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The average of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// source contains no elements.
        /// </exception>
        [Pure]
        public Amount Average(Func<TSource, Amount> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var total = decimal.Zero;
            var count = 0L;

            foreach (var item in source)
            {
                count++;
                var amount = selector(item);
                checked
                {
                    total += (decimal)amount;
                }
            }

            return count == 0
                ? throw NoElements()
                : (total / count).Amount();
        }
    }

    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the average of a sequence of nullable <see cref="Amount" /> values that are
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The average of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Amount? Average(Func<TSource, Amount?> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var total = decimal.Zero;
            var count = 0L;

            foreach (var item in source)
            {
                var value = selector(item);

                if (value.HasValue)
                {
                    count++;
                    var amount = value.GetValueOrDefault();
                    checked
                    {
                        total += (decimal)amount;
                    }
                }
            }
            return count is 0
                ? null
                : (total / count).Amount();
        }
    }

    extension(IEnumerable<Amount> source)
    {
        /// <summary>Computes the average of a sequence of <see cref="Amount" /> values.</summary>
        /// <returns>
        /// The average of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// source contains no elements.
        /// </exception>
        [Pure]
        public Amount Average()
        {
            Guard.NotNull(source);

            var total = decimal.Zero;
            var count = 0L;

            foreach (var amount in source)
            {
                count++;
                checked
                {
                    total += (decimal)amount;
                }
            }
            return count == 0
                ? throw NoElements()
                : (total / count).Amount();
        }
    }

    extension(IEnumerable<Amount?> source)
    {
        /// <summary>Computes the average of a sequence of nullable <see cref="Amount" /> values.</summary>
        /// <returns>
        /// The average of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Amount? Average()
        {
            Guard.NotNull(source);

            var total = decimal.Zero;
            var count = 0L;

            foreach (var value in source)
            {
                if (value.HasValue)
                {
                    count++;
                    var amount = value.GetValueOrDefault();
                    checked
                    {
                        total += (decimal)amount;
                    }
                }
            }
            return count is 0
                ? null
                : (total / count).Amount();
        }
    }

    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the average of a sequence of <see cref="Money" /> values that are obtained
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The average of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// source contains no elements.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all money elements have the same currency.
        /// </exception>
        [Pure]
        public Money Average(Func<TSource, Money> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var total = Money.Zero;
            var count = 0L;

            foreach (var item in source)
            {
                count++;
                total = total.Add(selector(item), nameof(Average));
            }

            return count is 0
                ? throw NoElements()
                : total / count;
        }
    }

    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the average of a sequence of nullable <see cref="Money" /> values that are
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The average of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all money elements have the same currency.
        /// </exception>
        [Pure]
        public Money? Average(Func<TSource, Money?> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var total = Money.Zero;
            var count = 0L;

            foreach (var item in source)
            {
                if (selector(item) is { } money)
                {
                    count++;
                    total = total.Add(money, nameof(Average));
                }
            }
            return count is 0
                ? null
                : total / count;
        }
    }

    extension(IEnumerable<Money> source)
    {
        /// <summary>Computes the average of a sequence of <see cref="Money" /> values.</summary>
        /// <returns>
        /// The average of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// source contains no elements.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all money elements have the same currency.
        /// </exception>
        [Pure]
        public Money Average()
        {
            Guard.NotNull(source);

            var total = Money.Zero;
            var count = 0L;

            foreach (var money in source)
            {
                count++;
                total = total.Add(money, nameof(Average));
            }

            return count is 0
                ? throw NoElements()
                : total / count;
        }
    }

    extension(IEnumerable<Money?> source)
    {
        /// <summary>Computes the average of a sequence of nullable <see cref="Money" /> values.</summary>
        /// <returns>
        /// The average of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all money elements have the same currency.
        /// </exception>
        [Pure]
        public Money? Average()
        {
            Guard.NotNull(source);

            var total = Money.Zero;
            var count = 0L;

            foreach (var value in source)
            {
                if (value is { } money)
                {
                    count++;
                    total = total.Add(money, nameof(Average));
                }
            }
            return count is 0
                ? null
                : total / count;
        }
    }

    extension(IEnumerable<Elo> elos)
    {
        /// <summary>Gets the average Elo.</summary>
        [Pure]
        public Elo Average(elo => (double)elo);
    }
}

    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the average of a sequence of <see cref="Percentage" /> values that are obtained
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The average of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// source contains no elements.
        /// </exception>
        [Pure]
        public Percentage Average(Func<TSource, Percentage> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var total = decimal.Zero;
            var count = 0L;

            foreach (var item in source)
            {
                count++;
                var percentage = selector(item);
                checked
                {
                    total += (decimal)percentage;
                }
            }

            return count == 0
                ? throw NoElements()
                : Percentage.Create(total / count);
        }
    }

    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the average of a sequence of nullable <see cref="Percentage" /> values that are
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The average of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Percentage? Average(Func<TSource, Percentage?> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var total = decimal.Zero;
            var count = 0L;

            foreach (var item in source)
            {
                if (selector(item) is Percentage percentage)
                {
                    count++;
                    checked
                    {
                        total += (decimal)percentage;
                    }
                }
            }
            return count is 0
                ? null
                : Percentage.Create(total / count);
        }
    }

    extension(IEnumerable<Percentage> source)
    {
        /// <summary>Computes the average of a sequence of <see cref="Percentage" /> values.</summary>
        /// <returns>
        /// The average of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// source contains no elements.
        /// </exception>
        [Pure]
        public Percentage Average()
        {
            Guard.NotNull(source);

            var total = decimal.Zero;
            var count = 0L;

            foreach (var percentage in source)
            {
                count++;
                checked
                {
                    total += (decimal)percentage;
                }
            }
            return count == 0
                ? throw NoElements()
                : Percentage.Create(total / count);
        }
    }

    extension(IEnumerable<Percentage?> source)
    {
        /// <summary>Computes the average of a sequence of nullable <see cref="Percentage" /> values.</summary>
        /// <returns>
        /// The average of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Percentage? Average()
        {
            Guard.NotNull(source);

            var total = decimal.Zero;
            var count = 0L;

            foreach (var value in source)
            {
                if (value is Percentage percentage)
                {
                    count++;
                    checked
                    {
                        total += (decimal)percentage;
                    }
                }
            }
            return count is 0
                ? null
                : Percentage.Create(total / count);
        }
    }

    extension(IEnumerable<StreamSize> streamSizes)
    {
        /// <summary>Computes the average of a sequence of stream sizes.</summary>
        [Pure]
        public StreamSize Average()
            => new((long)streamSizes.Average(streamSize => (long)streamSize));
    }

    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the sum of a sequence of <see cref="Amount" /> values that are obtained
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The sum of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Amount Sum(Func<TSource, Amount> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var total = decimal.Zero;

            foreach (var item in source)
            {
                var amount = selector(item);
                checked
                {
                    total += (decimal)amount;
                }
            }

            return total.Amount();
        }
    }

    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the sum of a sequence of nullable <see cref="Amount" /> values that are
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The sum of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Amount? Sum(Func<TSource, Amount?> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var total = decimal.Zero;
            var none = true;

            foreach (var item in source)
            {
                var value = selector(item);

                if (value.HasValue)
                {
                    none = false;
                    var amount = value.GetValueOrDefault();
                    checked
                    {
                        total += (decimal)amount;
                    }
                }
            }
            return none ? null : total.Amount();
        }
    }

    extension(IEnumerable<Amount> source)
    {
        /// <summary>Computes the sum of a sequence of <see cref="Amount" /> values.</summary>
        /// <returns>
        /// The sum of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Amount Sum()
        {
            Guard.NotNull(source);

            var total = decimal.Zero;

            foreach (var amount in source)
            {
                checked
                {
                    total += (decimal)amount;
                }
            }
            return total.Amount();
        }
    }

    extension(IEnumerable<Amount?> source)
    {
        /// <summary>Computes the sum of a sequence of nullable <see cref="Amount" /> values.</summary>
        /// <returns>
        /// The sum of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Amount? Sum()
        {
            Guard.NotNull(source);

            var total = decimal.Zero;
            var none = true;

            foreach (var value in source)
            {
                if (value.HasValue)
                {
                    none = false;
                    var amount = value.GetValueOrDefault();
                    checked
                    {
                        total += (decimal)amount;
                    }
                }
            }
            return none ? null : total.Amount();
        }
    }

    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the sum of a sequence of <see cref="Money" /> values that are obtained
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The sum of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all money elements have the same currency.
        /// </exception>
        [Pure]
        public Money Sum(Func<TSource, Money> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var currency = Currency.Empty;
            var first = true;
            var total = decimal.Zero;

            foreach (var item in source)
            {
                var money = selector(item);

                if (first)
                {
                    first = false;
                    currency = money.Currency;
                }
                if (currency != money.Currency)
                {
                    throw new CurrencyMismatchException(currency, money.Currency, nameof(Sum));
                }
                checked
                {
                    total += (decimal)money;
                }
            }
            return total + currency;
        }
    }

    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the sum of a sequence of nullable <see cref="Money" /> values that are
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The sum of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all money elements have the same currency.
        /// </exception>
        [Pure]
        public Money? Sum(Func<TSource, Money?> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var currency = Currency.Empty;
            var first = true;
            var total = decimal.Zero;
            var none = true;

            foreach (var item in source)
            {
                var value = selector(item);

                if (value.HasValue)
                {
                    none = false;
                    var money = value.GetValueOrDefault();

                    if (first)
                    {
                        first = false;
                        currency = money.Currency;
                    }
                    if (currency != money.Currency)
                    {
                        throw new CurrencyMismatchException(currency, money.Currency, nameof(Sum));
                    }
                    checked
                    {
                        total += (decimal)money;
                    }
                }
            }
            return none ? null : total + currency;
        }
    }

    extension(IEnumerable<Money> source)
    {
        /// <summary>Computes the sum of a sequence of <see cref="Money" /> values.</summary>
        /// <returns>
        /// The sum of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all money elements have the same currency.
        /// </exception>
        [Pure]
        public Money Sum()
        {
            Guard.NotNull(source);

            var currency = Currency.Empty;
            var first = true;
            var total = decimal.Zero;

            foreach (var money in source)
            {
                if (first)
                {
                    first = false;
                    currency = money.Currency;
                }
                if (currency != money.Currency)
                {
                    throw new CurrencyMismatchException(currency, money.Currency, nameof(Sum));
                }
                checked
                {
                    total += (decimal)money;
                }
            }
            return total + currency;
        }
    }

    extension(IEnumerable<Money?> source)
    {
        /// <summary>Computes the sum of a sequence of nullable <see cref="Money" /> values.</summary>
        /// <returns>
        /// The sum of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all money elements have the same currency.
        /// </exception>
        [Pure]
        public Money? Sum()
        {
            Guard.NotNull(source);

            var currency = Currency.Empty;
            var first = true;
            var total = decimal.Zero;
            var none = true;

            foreach (var value in source)
            {
                if (value.HasValue)
                {
                    none = false;
                    var money = value.GetValueOrDefault();

                    if (first)
                    {
                        first = false;
                        currency = money.Currency;
                    }
                    if (currency != money.Currency)
                    {
                        throw new CurrencyMismatchException(currency, money.Currency, nameof(Sum));
                    }
                    checked
                    {
                        total += (decimal)money;
                    }
                }
            }
            return none ? null : total + currency;
        }
    }

    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the sum of a sequence of <see cref="Percentage" /> values that are obtained
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The sum of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Percentage Sum(Func<TSource, Percentage> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var total = decimal.Zero;

            foreach (var item in source)
            {
                var percentage = selector(item);
                checked
                {
                    total += (decimal)percentage;
                }
            }

            return Percentage.Create(total);
        }
    }

    /// <typeparam name="TSource">
    /// The type of the elements of source.
    /// </typeparam>
    extension<TSource>(IEnumerable<TSource> source)
    {
        /// <summary>Computes the sum of a sequence of nullable <see cref="Percentage" /> values that are
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <returns>
        /// The sum of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Percentage? Sum(Func<TSource, Percentage?> selector)
        {
            Guard.NotNull(source);
            Guard.NotNull(selector);

            var total = decimal.Zero;
            var none = true;

            foreach (var item in source)
            {
                var value = selector(item);

                if (value is Percentage percentage)
                {
                    none = false;
                    checked
                    {
                        total += (decimal)percentage;
                    }
                }
            }
            return none ? null : Percentage.Create(total);
        }
    }

    extension(IEnumerable<Percentage> source)
    {
        /// <summary>Computes the sum of a sequence of <see cref="Percentage" /> values.</summary>
        /// <returns>
        /// The sum of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Percentage Sum()
        {
            Guard.NotNull(source);

            var total = decimal.Zero;

            foreach (var percentage in source)
            {
                checked
                {
                    total += (decimal)percentage;
                }
            }
            return Percentage.Create(total);
        }
    }

    extension(IEnumerable<Percentage?> source)
    {
        /// <summary>Computes the sum of a sequence of nullable <see cref="Percentage" /> values.</summary>
        /// <returns>
        /// The sum of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        [Pure]
        public Percentage? Sum()
        {
            Guard.NotNull(source);

            var total = decimal.Zero;
            var none = true;

            foreach (var value in source)
            {
                if (value is Percentage percentage)
                {
                    none = false;
                    checked
                    {
                        total += (decimal)percentage;
                    }
                }
            }
            return none ? null : Percentage.Create(total);
        }
    }

    extension(IEnumerable<StreamSize> streamSizes)
    {
        /// <summary>Computes the sum of a sequence of stream sizes.</summary>
        [Pure]
        public StreamSize Sum()
            => new(streamSizes.Sum(streamSize => (long)streamSize));
    }

    [Pure]
    private static InvalidOperationException NoElements() => new(QowaivMessages.InvalidOperationException_NoElements);
}
