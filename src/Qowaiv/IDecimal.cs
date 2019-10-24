namespace Qowaiv
{
    public interface IDecimal<TSvo> where TSvo : struct
    {
        TSvo Plus();
        TSvo Negate();

        TSvo Increment();
        TSvo Decrement();

        TSvo Add(TSvo other);
        TSvo Add(Percentage p);
       
        TSvo Subtract(TSvo other);
        TSvo Subtract(Percentage p);

        TSvo Multiply(short factor);
        TSvo Multiply(int factor);
        TSvo Multiply(long factor);
        TSvo Multiply(ushort factor);
        TSvo Multiply(uint factor);
        TSvo Multiply(ulong factor);
        TSvo Multiply(double factor);
        TSvo Multiply(decimal factor);
        TSvo Multiply(Percentage factor);

        TSvo Divide(short factor);
        TSvo Divide(int factor);
        TSvo Divide(long factor);
        TSvo Divide(ushort factor);
        TSvo Divide(uint factor);
        TSvo Divide(ulong factor);
        TSvo Divide(double factor);
        TSvo Divide(decimal factor);
        TSvo Divide(Percentage factor);

        TSvo Round();
        TSvo Round(int decimals);
        TSvo Round(DecimalRounding mode);
        TSvo Round(int decimals, DecimalRounding mode);
        TSvo RoundToMultiple(decimal multipleOf);
        TSvo RoundToMultiple(decimal multipleOf, DecimalRounding mode);
    }
}
