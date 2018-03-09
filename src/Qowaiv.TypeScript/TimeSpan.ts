module Qowaiv {
    /**
     * Represents a (second based) time span.
     */
    export class TimeSpan implements IEquatable, IFormattable, IJsonStringifyable {
        /**
         * Represents the pattern of a (potential) valid time span.
         * @remarks [ extra ][ d ][ hours ][ min ][ sec ][ ms ]
         */
        private static pattern = /^\d*((((\d+:(2[0-3]|[0-1]\d)|\d)?:[0-5])?\d:)?[0-5])?\d([,\.]\d+)?$/;

        /**
         * The underlying total of seconds.
         */
        private v: number;

        /**
         * @constructor
         */
        constructor(d?: number, h?: number, m?: number, s?:number, f?: number) {
            this.v = this.num(d) * 86400;
            this.v += this.num(h) * 3600;
            this.v += this.num(m) * 60;
            this.v += this.num(s);
            this.v += this.num(f) * 0.001;
        }

        private num(n: number): number {
            return isNaN(n) ? 0 : n;
        }

        /**
         * Returns the days of the time span.
         */
        public getDays(): number {
            return ~~(this.v / 86400);
        }
        /**
         * Returns the hours of the time span.
         */
        public getHours(): number {
            return ~~((this.v / 3600) % 24);
        }
        /**
         * Returns the minutes of the time span.
         */
        public getMinutes(): number {
            return ~~((this.v / 60) % 60);
        }
        /**
         * Returns the seconds of the time span.
         */
        public getSeconds(): number {
            return ~~(this.v % 60);
        }
        /**
         * Returns the milliseconds of the time span.
         */
        public getMilliseconds(): number {
            return ~~((this.v * 1000) % 1000);
        }
        /**
         * Returns the total of days of the time span.
         */
        public getTotalDays(): number {
            return this.v / 86400.0;
        }
        /**
         * Returns the total of hours of the time span.
         */
        public getTotalHours(): number {
            return this.v / 3600.0;
        }
        /**
         * Returns the total of minutes of the time span.
         */
        public getTotalMinutes(): number {
            return this.v / 60.0;
        }
        /**
         * Returns the total of seconds of the time span.
         */
        public getTotalSeconds(): number {
            return this.v;
        }
        /**
         * Returns the total of milliseconds of the time span.
         */
        public getTotalMilliseconds(): number {
            return this.v * 1000.0;
        }

        /**
         * Multiplies the time span with the specified factor.
         */
        public multiply(factor: number): TimeSpan {
            if (isNaN(factor)) { throw Error("factor is not a number."); }

            return TimeSpan.fromSeconds(this.v * factor);
        }

        /**
         * Divide the time span with the specified factor.
         */
        public divide(factor: number): TimeSpan {
            if (isNaN(factor)) { throw Error("factor is not a number."); }
            if (factor === 0) { throw Error("factor is zero."); }

            return TimeSpan.fromSeconds(this.v / factor);
        }

        /**
         * Returns a string that represents the current time span.
         */
        public toString(): string {
            var s = this.getTotalSeconds() % 60;
            var m = this.getMinutes();
            var h = this.getHours();
            var d = this.getDays();
            var str = '';
            if (str !== '' || d !== 0) { str += d + ':'; }
            if (str !== '' && h < 10) { str += '0'; }
            if (str !== '' || h !== 0) { str += h + ':'; }
            if (str !== '' && m < 10) { str += '0'; }
            if (str !== '' || m !== 0) { str += m + ':'; }
            if (str !== '' && s < 10) { str += '0'; }
            if (str !== '' || s !== 0) { str += s; }
            return str;
        }

        public format(format?: string): string {

            var sec = Math.round(this.v);
            var ts = TimeSpan.fromSeconds(sec);
            return ts.toString();
        }

        /**
         * Returns a value representing the current time span for JSON.
         * @remarks Is used by JSON.stringify().
         */
        public toJSON(): string {
            return this.toString();
        }

        /**
         * Creates a TimeSpan from a JSON string.
         * @param {string} s A string containing TimeSpan to convert.
         * @returns TimeSpan if valid, otherwise null.
         */
        public static fromJSON(s: string): TimeSpan {
            return TimeSpan.parse(s);
        }
        /**
        * Returns true if other is not null or undefined and a TimeSpan
        * representing the same value, otherwise false.
        */
        public equals(other: any): boolean {
            return other !== null &&
                other !== undefined &&
                other instanceof (TimeSpan) &&
                other.v === this.v;
        }

        /**
         * Returns true if the value represents valid time span, otherwise false.
         * @param {string} s A string containing time span.
         * @remarks This method calls create(). It's of no use, to call isValid(),
         * to avoid a create() call.
         */
        public static isValid(s: string): boolean {
            return typeof (s) === 'string' && TimeSpan.pattern.test(s);
        }

        /**
         * Creates a time span.
         * @param {string} s A string containing time span.
         * @returns A TimeSpan if valid, otherwise null.
         */
        public static parse(str: string): TimeSpan {
            if (TimeSpan.isValid(str)) {
                var bl = str.split(':');
                var l = bl.length;

                var s = parseFloat(bl[l - 1].replace(',', '.'));
                var d = (l > 3) ? parseInt(bl[l - 4]) : 0;
                var h = (l > 2) ? parseInt(bl[l - 3]) : 0;
                var m = (l > 1) ? parseInt(bl[l - 2]) : 0;
                return new TimeSpan(d, h, m, s);
            }
            return null;
        }
        /**
         * Creates a time span based on the specified seconds.
        */
        public static fromSeconds(seconds: number) {
            if (isNaN(seconds)) { throw Error("seconds is not a number."); }
            return new TimeSpan(0, 0, 0, seconds);
        }
    }
}
