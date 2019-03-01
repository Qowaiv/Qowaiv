declare module Qowaiv {
    class Guid implements IEquatable, IFormattable, IJsonStringifyable {
        constructor();
        private v;
        toString(): string;
        format(f?: string): string;
        toJSON(): string;
        version(): number;
        equals(other: any): boolean;
        static fromJSON(s: string): Guid;
        static isValid(s: string): boolean;
        static parse(s: string): Guid;
        private static strip;
        static empty(): Guid;
        static newGuid(seed?: Guid): Guid;
        private static rndGuid;
    }
}
interface IEquatable {
    equals(other: any): boolean;
}
interface IFormattable {
    toString(): string;
    format(f: string): string;
}
interface IJsonStringifyable {
    toJSON(): string;
}
declare module Qowaiv {
    class TimeSpan implements IEquatable, IFormattable, IJsonStringifyable {
        private static pattern;
        private v;
        constructor(d?: number, h?: number, m?: number, s?: number, f?: number);
        private num;
        getDays(): number;
        getHours(): number;
        getMinutes(): number;
        getSeconds(): number;
        getMilliseconds(): number;
        getTotalDays(): number;
        getTotalHours(): number;
        getTotalMinutes(): number;
        getTotalSeconds(): number;
        getTotalMilliseconds(): number;
        multiply(factor: number): TimeSpan;
        divide(factor: number): TimeSpan;
        toString(): string;
        format(format?: string): string;
        toJSON(): string;
        static fromJSON(s: string): TimeSpan;
        equals(other: any): boolean;
        static isValid(s: string): boolean;
        static parse(str: string): TimeSpan;
        static fromSeconds(seconds: number): TimeSpan;
    }
}
