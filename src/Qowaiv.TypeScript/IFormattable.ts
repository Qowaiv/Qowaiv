/**
 * Provides functionality to format the value of an object into a string representation.
 */
interface IFormattable
{
    /**
     * Returns a string that represents the object.
     * @returns string.
     */
    toString(): string;

    /**
     * Returns a formatted string that represents the object.
     * @param {string} f The format that this describes the formatting.
     * @returns formatted string.
     */
    format(f: string): string;
}