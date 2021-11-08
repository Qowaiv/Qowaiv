using Qowaiv;
using Qowaiv.Financial;
using Qowaiv.IO;
using Qowaiv.Statistics;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System.Linq
{
    /// <summary>Extensions on <see cref="IEnumerable{T}"/>.</summary>
    public static class QowaivEnumerableExtensions
    {
        /// <summary>Computes the average of a sequence of <see cref="Amount"/> values that are obtained
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an average.
        /// </param>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
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
        /// not all amount elements have the same currency.
        /// </exception>
        [Pure]
        public static Amount Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Amount> selector)
        {
            Guard.NotNull(source, nameof(source));

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

        /// <summary>Computes the average of a sequence of nullable <see cref="Amount"/> values that are
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an average.
        /// </param>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
        /// <returns>
        /// The average of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all amount elements have the same currency.
        /// </exception>
        [Pure]
        public static Amount? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Amount?> selector)
        {
            Guard.NotNull(source, nameof(source));

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
            if (count == 0)
            {
                return null;
            }
            return (total / count).Amount();
        }

        /// <summary>Computes the average of a sequence of <see cref="Amount"/> values.</summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an average.
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
        /// not all amount elements have the same currency.
        /// </exception>
        [Pure]
        public static Amount Average(this IEnumerable<Amount> source)
        {
            Guard.NotNull(source, nameof(source));

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

        /// <summary>Computes the average of a sequence of nullable <see cref="Amount"/> values.</summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an average.
        /// </param>
        /// <returns>
        /// The average of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all amount elements have the same currency.
        /// </exception>
        [Pure]
        public static Amount? Average(this IEnumerable<Amount?> source)
        {
            Guard.NotNull(source, nameof(source));

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
            if (count == 0)
            {
                return null;
            }
            return (total / count).Amount();
        }

        /// <summary>Computes the average of a sequence of <see cref="Money"/> values that are obtained
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an average.
        /// </param>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
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
        public static Money Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Money> selector)
        {
            Guard.NotNull(source, nameof(source));

            var currency = Currency.Empty;
            var first = true;
            var total = decimal.Zero;
            var count = 0L;

            foreach (var item in source)
            {
                count++;
                var money = selector(item);

                if (first)
                {
                    first = false;
                    currency = money.Currency;
                }
                if (currency != money.Currency)
                {
                    throw new CurrencyMismatchException(currency, money.Currency, nameof(Average));
                }
                checked
                {
                    total += (decimal)money;
                }
            }

            return count == 0
                ? throw NoElements()
                : (total / count) + currency;
        }

        /// <summary>Computes the average of a sequence of nullable <see cref="Money"/> values that are
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an average.
        /// </param>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
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
        public static Money? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Money?> selector)
        {
            Guard.NotNull(source, nameof(source));

            var currency = Currency.Empty;
            var first = true;
            var total = decimal.Zero;
            var count = 0L;

            foreach (var item in source)
            {
                var value = selector(item);

                if (value.HasValue)
                {
                    count++;
                    var money = value.GetValueOrDefault();

                    if (first)
                    {
                        first = false;
                        currency = money.Currency;
                    }
                    if (currency != money.Currency)
                    {
                        throw new CurrencyMismatchException(currency, money.Currency, nameof(Average));
                    }
                    checked
                    {
                        total += (decimal)money;
                    }
                }
            }
            if (count == 0)
            {
                return null;
            }
            return (total / count) + currency;
        }

        /// <summary>Computes the average of a sequence of <see cref="Money"/> values.</summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an average.
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
        public static Money Average(this IEnumerable<Money> source)
        {
            Guard.NotNull(source, nameof(source));

            var currency = Currency.Empty;
            var first = true;
            var total = decimal.Zero;
            var count = 0L;

            foreach (var money in source)
            {
                count++;

                if (first)
                {
                    first = false;
                    currency = money.Currency;
                }
                if (currency != money.Currency)
                {
                    throw new CurrencyMismatchException(currency, money.Currency, nameof(Average));
                }
                checked
                {
                    total += (decimal)money;
                }
            }

            return count == 0
                ? throw NoElements()
                : (total / count) + currency;
        }

        /// <summary>Computes the average of a sequence of nullable <see cref="Money"/> values.</summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an average.
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
        public static Money? Average(this IEnumerable<Money?> source)
        {
            Guard.NotNull(source, nameof(source));

            var currency = Currency.Empty;
            var first = true;
            var total = decimal.Zero;
            var count = 0L;

            foreach (var value in source)
            {
                if (value.HasValue)
                {
                    count++;
                    var money = value.GetValueOrDefault();

                    if (first)
                    {
                        first = false;
                        currency = money.Currency;
                    }
                    if (currency != money.Currency)
                    {
                        throw new CurrencyMismatchException(currency, money.Currency, nameof(Average));
                    }
                    checked
                    {
                        total += (decimal)money;
                    }
                }
            }
            if (count == 0)
            {
                return null;
            }
            return (total / count) + currency;
        }

        /// <summary>Gets the average Elo.</summary>
        [Pure]
        public static Elo Average(this IEnumerable<Elo> elos) => elos.Select(elo => (double)elo).Average();

        /// <summary>Computes the average of a sequence of stream sizes.</summary>
        [Pure]
        public static StreamSize Average(this IEnumerable<StreamSize> streamSizes)
            => new((long)streamSizes.Average(streamSize => (long)streamSize));


        /// <summary>Computes the sum of a sequence of <see cref="Amount"/> values that are obtained
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an sum.
        /// </param>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
        /// <returns>
        /// The sum of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// source contains no elements.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all amount elements have the same currency.
        /// </exception>
        [Pure]
        public static Amount Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Amount> selector)
        {
            Guard.NotNull(source, nameof(source));

            var total = decimal.Zero;
            var none = true;

            foreach (var item in source)
            {
                none = false;
                var amount = selector(item);
                checked
                {
                    total += (decimal)amount;
                }
            }

            return none
                ? throw NoElements()
                : total.Amount();
        }

        /// <summary>Computes the sum of a sequence of nullable <see cref="Amount"/> values that are
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an sum.
        /// </param>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
        /// <returns>
        /// The sum of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all amount elements have the same currency.
        /// </exception>
        [Pure]
        public static Amount? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Amount?> selector)
        {
            Guard.NotNull(source, nameof(source));

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
            if (none) return null;
            else return total.Amount();
        }

        /// <summary>Computes the sum of a sequence of <see cref="Amount"/> values.</summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an sum.
        /// </param>
        /// <returns>
        /// The sum of the sequence of values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// source contains no elements.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all amount elements have the same currency.
        /// </exception>
        [Pure]
        public static Amount Sum(this IEnumerable<Amount> source)
        {
            Guard.NotNull(source, nameof(source));

            var total = decimal.Zero;
            var none = true;

            foreach (var amount in source)
            {
                none = false;
                checked
                {
                    total += (decimal)amount;
                }
            }
            return none
                ? throw NoElements()
                : total.Amount();
        }

        /// <summary>Computes the sum of a sequence of nullable <see cref="Amount"/> values.</summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an sum.
        /// </param>
        /// <returns>
        /// The sum of the sequence of values, or null if the source sequence is empty
        /// or contains only values that are null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or selector is null.
        /// </exception>
        /// <exception cref="CurrencyMismatchException">
        /// not all amount elements have the same currency.
        /// </exception>
        [Pure]
        public static Amount? Sum(this IEnumerable<Amount?> source)
        {
            Guard.NotNull(source, nameof(source));

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
            if (none) return null;
            else return total.Amount();
        }


        /// <summary>Computes the sum of a sequence of <see cref="Money"/> values that are obtained
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an sum.
        /// </param>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
        /// <returns>
        /// The sum of the sequence of values.
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
        public static Money Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Money> selector)
        {
            Guard.NotNull(source, nameof(source));

            var currency = Currency.Empty;
            var first = true;
            var total = decimal.Zero;
            var none = true;

            foreach (var item in source)
            {
                none = false;
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

            return none
                ? throw NoElements()
                : total + currency;
        }

        /// <summary>Computes the sum of a sequence of nullable <see cref="Money"/> values that are
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an sum.
        /// </param>
        /// <param name="selector">
        /// A transform function to apply to each element.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
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
        public static Money? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Money?> selector)
        {
            Guard.NotNull(source, nameof(source));

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
            if (none) return null;
            else return total + currency;
        }

        /// <summary>Computes the sum of a sequence of <see cref="Money"/> values.</summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an sum.
        /// </param>
        /// <returns>
        /// The sum of the sequence of values.
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
        public static Money Sum(this IEnumerable<Money> source)
        {
            Guard.NotNull(source, nameof(source));

            var currency = Currency.Empty;
            var first = true;
            var total = decimal.Zero;
            var none = true;

            foreach (var money in source)
            {
                none = false;

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

            if (none)
            {
                throw NoElements();
            }
            return total + currency;
        }

        /// <summary>Computes the sum of a sequence of nullable <see cref="Money"/> values.</summary>
        /// <param name="source">
        /// A sequence of values that are used to calculate an sum.
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
        public static Money? Sum(this IEnumerable<Money?> source)
        {
            Guard.NotNull(source, nameof(source));

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
            if (none) return null;
            else return total + currency;
        }

        /// <summary>Computes the sum of a sequence of stream sizes.</summary>
        [Pure]
        public static StreamSize Sum(this IEnumerable<StreamSize> streamSizes)
            => new(streamSizes.Sum(streamSize => (long)streamSize));

        /// <summary>Converts the enumeration into an <see cref="EmailAddressCollection"/>.</summary>
        [Pure]
        public static EmailAddressCollection ToCollection(this IEnumerable<EmailAddress> adresses)
            => new(adresses);

        [Pure]
        private static InvalidOperationException NoElements() => new(QowaivMessages.InvalidOperationException_NoElements);
    }
}
