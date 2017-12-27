declare module Qowaiv {
    /**
     * Represents a Globally unique identifier (GUID).
     */
    class Guid implements IEquatable, IFormattable, IJsonStringifyable {
        /**
         * @constructor
         * @remarks It is the default constructor, for creating an actual GUID
         *          you will normally use Guid.newGuid() or Guid.parse(string).
         */
        constructor();
        /**
         * The underlying value.
         */
        private v;
        /**
         * Returns a string that represents the current GUID.
         */
        toString(): string;
        /**
         * Returns a string that represents the current GUID.
         */
        format(f?: string): string;
        /**
         * Returns a JSON representation of the GUID.
         */
        toJSON(): string;
        /**
         * Returns the version of the GUID.
         */
        version(): number;
        /**
         * Returns true if other is not null or undefined and a GUID
         * representing the same value, otherwise false.
         */
        equals(other: any): boolean;
        /**
          * Creates a GUID from a JSON string.
          * @param {string} s A JSON string representing the GUID.
          * @returns A GUID if valid, otherwise null.
          */
        static fromJSON(s: string): Guid;
        /**
         * Returns true if the val represents valid GUID, otherwise false.
         * @param {string} s A string containing GUID.
         * @remarks This method calls create(). It's of no use, to call isValid(),
         *          to avoid a create() call.
         */
        static isValid(s: string): boolean;
        /**
         * Creates a GUID.
         * @param {string} s A string containing GUID to convert or a number.
         * @returns A GUID if valid, otherwise null.
         */
        static parse(s: string): Guid;
        /**
         * Returns a new empty GUID.
         */
        static empty(): Guid;
        /**
         * Creates a GUID.
         * @returns A random GUID.
         */
        static newGuid(seed?: Guid): Guid;
        /**
         * Creates random GUID blocks.
         * @remarks called 4 times by Guid.newGuid().
         */
        private static rndGuid(s);
    }
}
/**
 * Defines a generalized method that for determining equality of instances.
 */
interface IEquatable {
    /**
     * Returns true if other is not null or undefined and a object
     * representing the same value, otherwise false.
     */
    equals(other: any): boolean;
}
/**
 * Provides functionality to format the value of an object into a string representation.
 */
interface IFormattable {
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
/**
 * To support JSON.stringify()
 */
interface IJsonStringifyable {
    /**
     * Returns a JSON representation of the object.
     */
    toJSON(): string;
}
declare module Qowaiv {
    /**
     * Represents a (second based) time span.
     */
    class TimeSpan implements IEquatable, IFormattable, IJsonStringifyable {
        /**
         * Represents the pattern of a (potential) valid time span.
         * @remarks [ extra ][ d ][ hours ][ min ][ sec ][ ms ]
         */
        private static pattern;
        /**
         * The underlying total of seconds.
         */
        private v;
        /**
         * @constructor
         */
        constructor(d?: number, h?: number, m?: number, s?: number, f?: number);
        private num(n);
        /**
         * Returns the days of the time span.
         */
        getDays(): number;
        /**
         * Returns the hours of the time span.
         */
        getHours(): number;
        /**
         * Returns the minutes of the time span.
         */
        getMinutes(): number;
        /**
         * Returns the seconds of the time span.
         */
        getSeconds(): number;
        /**
         * Returns the milliseconds of the time span.
         */
        getMilliseconds(): number;
        /**
         * Returns the total of days of the time span.
         */
        getTotalDays(): number;
        /**
         * Returns the total of hours of the time span.
         */
        getTotalHours(): number;
        /**
         * Returns the total of minutes of the time span.
         */
        getTotalMinutes(): number;
        /**
         * Returns the total of seconds of the time span.
         */
        getTotalSeconds(): number;
        /**
         * Returns the total of milliseconds of the time span.
         */
        getTotalMilliseconds(): number;
        /**
         * Returns the total of ticks of the time span.
         */
        getTicks(): number;
        /**
         * Multiplies the time span with the specified factor.
         */
        multiply(factor: number): TimeSpan;
        /**
         * Divide the time span with the specified factor.
         */
        divide(factor: number): TimeSpan;
        /**
         * Returns a string that represents the current time span.
         */
        toString(): string;
        format(format?: string): string;
        /**
         * Returns a value representing the current time span for JSON.
         * @remarks Is used by JSON.stringify().
         */
        toJSON(): string;
        /**
         * Creates a TimeSpan from a JSON string.
         * @param {string} s A string containing TimeSpan to convert.
         * @returns TimeSpan if valid, otherwise null.
         */
        static fromJSON(s: string): TimeSpan;
        /**
        * Returns true if other is not null or undefined and a TimeSpan
        * representing the same value, otherwise false.
        */
        equals(other: any): boolean;
        /**
         * Returns true if the value represents valid time span, otherwise false.
         * @param {string} s A string containing time span.
         * @remarks This method calls create(). It's of no use, to call isValid(),
         * to avoid a create() call.
         */
        static isValid(s: string): boolean;
        /**
         * Creates a time span.
         * @param {string} s A string containing time span.
         * @returns A TimeSpan if valid, otherwise null.
         */
        static parse(str: string): TimeSpan;
        /**
         * Creates a time span based on the specified seconds.
        */
        static fromSeconds(seconds: number): TimeSpan;
    }
}
