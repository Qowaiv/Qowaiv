namespace Qowaiv.EntityFrameworkCore.Converters;

internal sealed class AmountDecimalConverter() : ValueConverter<Amount, decimal>(
    svo => (decimal)svo,
    dec => dec.Amount());
