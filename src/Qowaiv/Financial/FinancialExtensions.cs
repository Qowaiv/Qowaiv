using System;
using System.Collections.Generic;

namespace Qowaiv.Financial
{
    public static class FinancialExtensions
    {
        /// Summary:
        ///     Computes the average of a sequence of System.Decimal values that are obtained
        ///     by invoking a transform function on each element of the input sequence.
        ///
        /// Parameters:
        ///   source:
        ///     A sequence of values that are used to calculate an average.
        ///
        ///   selector:
        ///     A transform function to apply to each element.
        ///
        /// Type parameters:
        ///   TSource:
        ///     The type of the elements of source.
        ///
        /// Returns:
        ///     The average of the sequence of values.
        ///
        /// Exceptions:
        ///   T:System.ArgumentNullException:
        ///     source or selector is null.
        ///
        ///   T:System.InvalidOperationException:
        ///     source contains no elements.
        ///
        ///   T:System.OverflowException:
        ///     The sum of the elements in the sequence is larger than System.Decimal.MaxValue.
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
                else if (currency != money.Currency)
                {
                    throw new CurrencyMismatchException(currency, money.Currency, nameof(Average));
                }

                checked
                {
                    total += (decimal)money;
                }
            }

            if (count == 0)
            {
                throw NoElements();
            }

            return (total / count) + currency;
        }

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
                    else if (currency != money.Currency)
                    {
                        throw new CurrencyMismatchException(currency, money.Currency, nameof(Average));
                    }

                    total += (decimal)value;
                }
            }
            if (count == 0)
            {
                throw new Exception();
            }
            return (total / count) + currency;
        }


        public static Money Average(this IEnumerable<Money> source)
        {

        }
        public static Money? Average(this IEnumerable<Money?> source)
        {

        }

        public static Money Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Money> selector)
        {
            Guard.NotNull(source, nameof(source));

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
                else if (currency != money.Currency)
                {
                    throw new CurrencyMismatchException(currency, money.Currency, nameof(Sum));
                }

                total += (decimal)money;
            }
            return total + currency;
        }

        public static Money? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Money?> selector)
        {
            Guard.NotNull(source, nameof(source));

            var currency = Currency.Empty;
            var first = true;
            var total = decimal.Zero;

            foreach (var item in source)
            {
                var value = selector(item);

                if (value.HasValue)
                {
                    var money = value.GetValueOrDefault();
                    if (first)
                    {
                        first = false;
                        currency = money.Currency;
                    }
                    else if (currency != money.Currency)
                    {
                        throw new CurrencyMismatchException(currency, money.Currency, nameof(Sum));
                    }

                    total += (decimal)value;
                }
            }
            return total + currency;
        }


        private static InvalidOperationException NoElements() => new InvalidOperationException(QowaivMessages.InvalidOperationException_NoElements);
    }
}
