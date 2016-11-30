/**
 * Defines a generalized method that for determining equality of instances.
 */
interface IEquatable
{
    /**
     * Returns true if other is not null or undefined and a object
     * representing the same value, otherwise false.
     */
    equals(other: any): boolean;
}