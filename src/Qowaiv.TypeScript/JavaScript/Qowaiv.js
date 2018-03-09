var Qowaiv;
(function (Qowaiv) {
    /**
     * Represents a Globally unique identifier (GUID).
     */
    class Guid {
        /**
         * @constructor
         * @remarks It is the default constructor, for creating an actual GUID
         *          you will normally use Guid.newGuid() or Guid.parse(string).
         */
        constructor() {
            /**
             * The underlying value.
             */
            this.v = '00000000-0000-0000-0000-000000000000';
        }
        /**
         * Returns a string that represents the current GUID.
         */
        toString() {
            return this.v;
        }
        /**
         * Returns a string that represents the current GUID.
         */
        format(f) {
            switch (f) {
                case 'B': return '{' + this.v + '}';
                case 'b': return '{' + this.v.toLowerCase() + '}';
                case 'S': return this.v.replace(/-/g, '');
                case 's': return this.v.replace(/-/g, '').toLowerCase();
                case 'l': return this.v.toLowerCase();
                case 'U':
                case 'u':
                default: return this.v;
            }
        }
        /**
         * Returns a JSON representation of the GUID.
         */
        toJSON() {
            return this.v;
        }
        /**
         * Returns the version of the GUID.
         */
        version() {
            return parseInt(this.v.substr(14, 1));
        }
        /**
         * Returns true if other is not null or undefined and a GUID
         * representing the same value, otherwise false.
         */
        equals(other) {
            return other !== null &&
                other !== undefined &&
                other instanceof (Guid) &&
                other.v === this.v;
        }
        /**
          * Creates a GUID from a JSON string.
          * @param {string} s A JSON string representing the GUID.
          * @returns A GUID if valid, otherwise null.
          */
        static fromJSON(s) {
            return Guid.parse(s);
        }
        /**
         * Returns true if the val represents valid GUID, otherwise false.
         * @param {string} s A string containing GUID.
         * @remarks This method calls create(). It's of no use, to call isValid(),
         *          to avoid a create() call.
         */
        static isValid(s) {
            return /^[0-9ABCDEF]{32}$/i.test(s.replace(/-/g, ''));
        }
        /**
         * Creates a GUID.
         * @param {string} s A string containing GUID to convert or a number.
         * @returns A GUID if valid, otherwise null.
         */
        static parse(s) {
            // an empty string should equal Guid.Empty.
            if (s === '') {
                return new Guid();
            }
            // if the value parameter is valid
            if (Guid.isValid(s)) {
                var guid = new Guid();
                s = s.replace(/-/g, '').toUpperCase();
                guid.v = s.replace(/(.{8})(.{4})(.{4})(.{4})(.{8})/, '$1-$2-$3-$4-$5');
                return guid;
            }
            // return null if creation failed.
            return null;
        }
        /**
         * Returns a new empty GUID.
         */
        static empty() {
            return new Guid();
        }
        /**
         * Creates a GUID.
         * @returns A random GUID.
         */
        static newGuid(seed) {
            var guid = new Guid();
            guid.v = (Guid.rndGuid(false) +
                Guid.rndGuid(true) +
                Guid.rndGuid(true) +
                Guid.rndGuid(false)).toUpperCase();
            if (seed !== null && seed instanceof (Guid)) {
                var lookup = '0123456789ABCDEF';
                var merged = '';
                for (var i = 0; i < 36; i++) {
                    var l = lookup.indexOf(seed.v.charAt(i));
                    var r = lookup.indexOf(guid.v.charAt(i));
                    merged += l === -1 || r === -1 ? guid.v.charAt(i) : lookup.charAt(l ^ r);
                }
            }
            // set version to 4 (Random).
            guid.v = guid.v.substr(0, 14) + '4' + guid.v.substr(15);
            return guid;
        }
        /**
         * Creates random GUID blocks.
         * @remarks called 4 times by Guid.newGuid().
         */
        static rndGuid(s) {
            var p = (Math.random().toString(16) + '000000000').substr(2, 8);
            return s ? '-' + p.substr(0, 4) + '-' + p.substr(4, 4) : p;
        }
    }
    Qowaiv.Guid = Guid;
})(Qowaiv || (Qowaiv = {}));
var Qowaiv;
(function (Qowaiv) {
    /**
     * Represents a (second based) time span.
     */
    class TimeSpan {
        /**
         * @constructor
         */
        constructor(d, h, m, s, f) {
            this.v = this.num(d) * 86400;
            this.v += this.num(h) * 3600;
            this.v += this.num(m) * 60;
            this.v += this.num(s);
            this.v += this.num(f) * 0.001;
        }
        num(n) {
            return isNaN(n) ? 0 : n;
        }
        /**
         * Returns the days of the time span.
         */
        getDays() {
            return ~~(this.v / 86400);
        }
        /**
         * Returns the hours of the time span.
         */
        getHours() {
            return ~~((this.v / 3600) % 24);
        }
        /**
         * Returns the minutes of the time span.
         */
        getMinutes() {
            return ~~((this.v / 60) % 60);
        }
        /**
         * Returns the seconds of the time span.
         */
        getSeconds() {
            return ~~(this.v % 60);
        }
        /**
         * Returns the milliseconds of the time span.
         */
        getMilliseconds() {
            return ~~((this.v * 1000) % 1000);
        }
        /**
         * Returns the total of days of the time span.
         */
        getTotalDays() {
            return this.v / 86400.0;
        }
        /**
         * Returns the total of hours of the time span.
         */
        getTotalHours() {
            return this.v / 3600.0;
        }
        /**
         * Returns the total of minutes of the time span.
         */
        getTotalMinutes() {
            return this.v / 60.0;
        }
        /**
         * Returns the total of seconds of the time span.
         */
        getTotalSeconds() {
            return this.v;
        }
        /**
         * Returns the total of milliseconds of the time span.
         */
        getTotalMilliseconds() {
            return this.v * 1000.0;
        }
        /**
         * Multiplies the time span with the specified factor.
         */
        multiply(factor) {
            if (isNaN(factor)) {
                throw Error("factor is not a number.");
            }
            return TimeSpan.fromSeconds(this.v * factor);
        }
        /**
         * Divide the time span with the specified factor.
         */
        divide(factor) {
            if (isNaN(factor)) {
                throw Error("factor is not a number.");
            }
            if (factor === 0) {
                throw Error("factor is zero.");
            }
            return TimeSpan.fromSeconds(this.v / factor);
        }
        /**
         * Returns a string that represents the current time span.
         */
        toString() {
            var s = this.getTotalSeconds() % 60;
            var m = this.getMinutes();
            var h = this.getHours();
            var d = this.getDays();
            var str = '';
            if (str !== '' || d !== 0) {
                str += d + ':';
            }
            if (str !== '' && h < 10) {
                str += '0';
            }
            if (str !== '' || h !== 0) {
                str += h + ':';
            }
            if (str !== '' && m < 10) {
                str += '0';
            }
            if (str !== '' || m !== 0) {
                str += m + ':';
            }
            if (str !== '' && s < 10) {
                str += '0';
            }
            if (str !== '' || s !== 0) {
                str += s;
            }
            return str;
        }
        format(format) {
            var sec = Math.round(this.v);
            var ts = TimeSpan.fromSeconds(sec);
            return ts.toString();
        }
        /**
         * Returns a value representing the current time span for JSON.
         * @remarks Is used by JSON.stringify().
         */
        toJSON() {
            return this.toString();
        }
        /**
         * Creates a TimeSpan from a JSON string.
         * @param {string} s A string containing TimeSpan to convert.
         * @returns TimeSpan if valid, otherwise null.
         */
        static fromJSON(s) {
            return TimeSpan.parse(s);
        }
        /**
        * Returns true if other is not null or undefined and a TimeSpan
        * representing the same value, otherwise false.
        */
        equals(other) {
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
        static isValid(s) {
            return typeof (s) === 'string' && TimeSpan.pattern.test(s);
        }
        /**
         * Creates a time span.
         * @param {string} s A string containing time span.
         * @returns A TimeSpan if valid, otherwise null.
         */
        static parse(str) {
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
        static fromSeconds(seconds) {
            if (isNaN(seconds)) {
                throw Error("seconds is not a number.");
            }
            return new TimeSpan(0, 0, 0, seconds);
        }
    }
    /**
     * Represents the pattern of a (potential) valid time span.
     * @remarks [ extra ][ d ][ hours ][ min ][ sec ][ ms ]
     */
    TimeSpan.pattern = /^\d*((((\d+:(2[0-3]|[0-1]\d)|\d)?:[0-5])?\d:)?[0-5])?\d([,\.]\d+)?$/;
    Qowaiv.TimeSpan = TimeSpan;
})(Qowaiv || (Qowaiv = {}));
//# sourceMappingURL=Qowaiv.js.map