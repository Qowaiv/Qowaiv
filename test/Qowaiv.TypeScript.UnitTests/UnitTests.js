var Qowaiv;
(function (Qowaiv) {
    /**
     * Represents a Globally unique identifier (GUID).
     */
    var Guid = /** @class */ (function () {
        /**
         * @constructor
         * @remarks It is the default constructor, for creating an actual GUID
         *          you will normally use Guid.newGuid() or Guid.parse(string).
         */
        function Guid() {
            /**
             * The underlying value.
             */
            this.v = '00000000-0000-0000-0000-000000000000';
        }
        /**
         * Returns a string that represents the current GUID.
         */
        Guid.prototype.toString = function () {
            return this.v;
        };
        /**
         * Returns a string that represents the current GUID.
         */
        Guid.prototype.format = function (f) {
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
        };
        /**
         * Returns a JSON representation of the GUID.
         */
        Guid.prototype.toJSON = function () {
            return this.v;
        };
        /**
         * Returns the version of the GUID.
         */
        Guid.prototype.version = function () {
            return parseInt(this.v.substr(14, 1));
        };
        /**
         * Returns true if other is not null or undefined and a GUID
         * representing the same value, otherwise false.
         */
        Guid.prototype.equals = function (other) {
            return other !== null &&
                other !== undefined &&
                other instanceof (Guid) &&
                other.v === this.v;
        };
        /**
          * Creates a GUID from a JSON string.
          * @param {string} s A JSON string representing the GUID.
          * @returns {Guid} A GUID if valid, otherwise null.
          */
        Guid.fromJSON = function (s) {
            return Guid.parse(s);
        };
        /**
         * Returns true if the val represents valid GUID, otherwise false.
         * @param {string} s A string containing GUID.
         * @remarks This method calls create(). It's of no use, to call isValid(),
         *          to avoid a create() call.
         */
        Guid.isValid = function (s) {
            return /^[0-9ABCDEF]{32}$/i.test(Guid.strip(s));
        };
        /**
         * Creates a GUID.
         * @param {string} s A string containing GUID to convert or a number.
         * @returns {Guid} A GUID if valid, otherwise null.
         */
        Guid.parse = function (s) {
            // an empty string should equal Guid.Empty.
            if (s === '') {
                return new Guid();
            }
            s = Guid.strip(s).toUpperCase();
            // if the value parameter is valid
            if (Guid.isValid(s)) {
                var guid = new Guid();
                guid.v = s.replace(/(.{8})(.{4})(.{4})(.{4})(.{12})/, '$1-$2-$3-$4-$5');
                return guid;
            }
            // return null if creation failed.
            return null;
        };
        Guid.strip = function (s) {
            var replace = s.replace(/-/g, '');
            if (replace.indexOf('{') == 0 && replace.lastIndexOf('}') == replace.length - 1) {
                replace = replace.substr(1, replace.length - 2);
            }
            return replace;
        };
        /**
         * Returns a new empty GUID.
         */
        Guid.empty = function () {
            return new Guid();
        };
        /**
         * Creates a GUID.
         * @returns {Guid} A random GUID.
         */
        Guid.newGuid = function (seed) {
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
        };
        /**
         * Creates random GUID blocks.
         * @remarks called 4 times by Guid.newGuid().
         */
        Guid.rndGuid = function (s) {
            var p = (Math.random().toString(16) + '000000000').substr(2, 8);
            return s ? '-' + p.substr(0, 4) + '-' + p.substr(4, 4) : p;
        };
        return Guid;
    }());
    Qowaiv.Guid = Guid;
})(Qowaiv || (Qowaiv = {}));
/// <reference path="Jasmine.d.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/IEquatable.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/IFormattable.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/IJsonStringifyable.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/Guid.ts" />
describe("GUID: ", function () {
    it("The version of newGuid() should be valid", function () {
        var guid = Qowaiv.Guid.newGuid();
        expect(Qowaiv.Guid.isValid(guid.toString())).toBeTruthy();
    });
    it("The version of newGuid(seed) should be valid", function () {
        var seed = Qowaiv.Guid.parse("DC7FBA65-DF6F-4CB9-8FAA-6C7B5654F189");
        var guid = Qowaiv.Guid.newGuid(seed);
        expect(Qowaiv.Guid.isValid(guid.toString())).toBeTruthy();
    });
    it("The version of newGuid() should be 4", function () {
        var guid = Qowaiv.Guid.newGuid();
        expect(guid.version()).toBe(4);
    });
    it("The version of some random guid should be 4", function () {
        var guid = Qowaiv.Guid.parse("DC7FBA65-DF6F-4CB9-8FAA-6C7B5654F189");
        expect(guid.version()).toBe(4);
    });
    it("The version of empty() should be 0", function () {
        var guid = Qowaiv.Guid.empty();
        expect(guid.version()).toBe(0);
    });
    it("format('B') should have brackets.", function () {
        var guid = Qowaiv.Guid.parse("DC7FBA65-DF6F-4CB9-8FAA-6C7B5654F189");
        expect(guid.format("B")).toBe("{DC7FBA65-DF6F-4CB9-8FAA-6C7B5654F189}");
    });
    it("format('b') should have brackets and be lowercase.", function () {
        var guid = Qowaiv.Guid.parse("DC7FBA65-DF6F-4CB9-8FAA-6C7B5654F189");
        expect(guid.format("b")).toBe("{dc7fba65-df6f-4cb9-8faa-6c7b5654f189}");
    });
    it("format('S') should have no dashes.", function () {
        var guid = Qowaiv.Guid.parse("DC7FBA65-DF6F-4CB9-8FAA-6C7B5654F189");
        expect(guid.format("S")).toBe("DC7FBA65DF6F4CB98FAA6C7B5654F189");
    });
    it("format('s') should have no dashed and be lowercase.", function () {
        var guid = Qowaiv.Guid.parse("DC7FBA65-DF6F-4CB9-8FAA-6C7B5654F189");
        expect(guid.format("s")).toBe("dc7fba65df6f4cb98faa6c7b5654f189");
    });
    it("Parse('{DC7FBA65-DF6F-4CB9-8FAA-6C7B5654F189}') should be parseable.", function () {
        var guid = Qowaiv.Guid.parse("{DC7FBA65-DF6F-4CB9-8FAA-6C7B5654F189}");
        expect(guid.format("U")).toBe("DC7FBA65-DF6F-4CB9-8FAA-6C7B5654F189");
    });
    it("Parse('Nonsense') should not be parseable.", function () {
        var guid = Qowaiv.Guid.parse("Nonsense");
        expect(guid).toBe(null);
    });
});
var Qowaiv;
(function (Qowaiv) {
    /**
     * Represents a (second based) time span.
     */
    var TimeSpan = /** @class */ (function () {
        /**
         * @constructor
         */
        function TimeSpan(d, h, m, s, f) {
            this.v = this.num(d) * 86400;
            this.v += this.num(h) * 3600;
            this.v += this.num(m) * 60;
            this.v += this.num(s);
            this.v += this.num(f) * 0.001;
        }
        TimeSpan.prototype.num = function (n) {
            return isNaN(n) ? 0 : n;
        };
        /**
         * Returns the days of the time span.
         */
        TimeSpan.prototype.getDays = function () {
            return ~~(this.v / 86400);
        };
        /**
         * Returns the hours of the time span.
         */
        TimeSpan.prototype.getHours = function () {
            return ~~((this.v / 3600) % 24);
        };
        /**
         * Returns the minutes of the time span.
         */
        TimeSpan.prototype.getMinutes = function () {
            return ~~((this.v / 60) % 60);
        };
        /**
         * Returns the seconds of the time span.
         */
        TimeSpan.prototype.getSeconds = function () {
            return ~~(this.v % 60);
        };
        /**
         * Returns the milliseconds of the time span.
         */
        TimeSpan.prototype.getMilliseconds = function () {
            return ~~((this.v * 1000) % 1000);
        };
        /**
         * Returns the total of days of the time span.
         */
        TimeSpan.prototype.getTotalDays = function () {
            return this.v / 86400.0;
        };
        /**
         * Returns the total of hours of the time span.
         */
        TimeSpan.prototype.getTotalHours = function () {
            return this.v / 3600.0;
        };
        /**
         * Returns the total of minutes of the time span.
         */
        TimeSpan.prototype.getTotalMinutes = function () {
            return this.v / 60.0;
        };
        /**
         * Returns the total of seconds of the time span.
         */
        TimeSpan.prototype.getTotalSeconds = function () {
            return this.v;
        };
        /**
         * Returns the total of milliseconds of the time span.
         */
        TimeSpan.prototype.getTotalMilliseconds = function () {
            return this.v * 1000.0;
        };
        /**
         * Multiplies the time span with the specified factor.
         */
        TimeSpan.prototype.multiply = function (factor) {
            if (isNaN(factor)) {
                throw Error("factor is not a number.");
            }
            return TimeSpan.fromSeconds(this.v * factor);
        };
        /**
         * Divide the time span with the specified factor.
         */
        TimeSpan.prototype.divide = function (factor) {
            if (isNaN(factor)) {
                throw Error("factor is not a number.");
            }
            if (factor === 0) {
                throw Error("factor is zero.");
            }
            return TimeSpan.fromSeconds(this.v / factor);
        };
        /**
         * Returns a string that represents the current time span.
         */
        TimeSpan.prototype.toString = function () {
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
        };
        TimeSpan.prototype.format = function (format) {
            var sec = Math.round(this.v);
            var ts = TimeSpan.fromSeconds(sec);
            return ts.toString();
        };
        /**
         * Returns a value representing the current time span for JSON.
         * @remarks Is used by JSON.stringify().
         */
        TimeSpan.prototype.toJSON = function () {
            return this.toString();
        };
        /**
         * Creates a TimeSpan from a JSON string.
         * @param {string} s A string containing TimeSpan to convert.
         * @returns TimeSpan if valid, otherwise null.
         */
        TimeSpan.fromJSON = function (s) {
            return TimeSpan.parse(s);
        };
        /**
        * Returns true if other is not null or undefined and a TimeSpan
        * representing the same value, otherwise false.
        */
        TimeSpan.prototype.equals = function (other) {
            return other !== null &&
                other !== undefined &&
                other instanceof (TimeSpan) &&
                other.v === this.v;
        };
        /**
         * Returns true if the value represents valid time span, otherwise false.
         * @param {string} s A string containing time span.
         * @remarks This method calls create(). It's of no use, to call isValid(),
         * to avoid a create() call.
         */
        TimeSpan.isValid = function (s) {
            return typeof (s) === 'string' && TimeSpan.pattern.test(s);
        };
        /**
         * Creates a time span.
         * @param {string} s A string containing time span.
         * @returns A TimeSpan if valid, otherwise null.
         */
        TimeSpan.parse = function (str) {
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
        };
        /**
         * Creates a time span based on the specified seconds.
        */
        TimeSpan.fromSeconds = function (seconds) {
            if (isNaN(seconds)) {
                throw Error("seconds is not a number.");
            }
            return new TimeSpan(0, 0, 0, seconds);
        };
        /**
         * Represents the pattern of a (potential) valid time span.
         * @remarks [ extra ][ d ][ hours ][ min ][ sec ][ ms ]
         */
        TimeSpan.pattern = /^\d*((((\d+:(2[0-3]|[0-1]\d)|\d)?:[0-5])?\d:)?[0-5])?\d([,\.]\d+)?$/;
        return TimeSpan;
    }());
    Qowaiv.TimeSpan = TimeSpan;
})(Qowaiv || (Qowaiv = {}));
/// <reference path="Jasmine.d.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/IEquatable.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/IFormattable.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/IJsonStringifyable.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/TimeSpan.ts" />
describe("TimeSpan: ", function () {
    it("getDays should be 1.", function () {
        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getDays()).toBe(1);
    });
    it("getHours should be 13.", function () {
        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getHours()).toBe(13);
    });
    it("getMinutes should be 15.", function () {
        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getMinutes()).toBe(15);
    });
    it("getSeconds should be 17.", function () {
        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getSeconds()).toBe(17);
    });
    it("getMilliseconds should be 19.", function () {
        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getMilliseconds()).toBe(19);
    });
    it("getTotalDays should be 1.5522803125", function () {
        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getTotalDays()).toBe(1.5522803125);
    });
    it("getTotalHours should be 37.2547275", function () {
        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getTotalHours()).toBe(37.2547275);
    });
    it("getTotalMinutes should be 2235.28365", function () {
        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getTotalMinutes()).toBe(2235.28365);
    });
    it("getTotalSeconds should be 134117.019", function () {
        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getTotalSeconds()).toBe(134117.019);
    });
    it("getTotalMilliseconds should be 134117019", function () {
        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getTotalMilliseconds()).toBe(134117019);
    });
});
