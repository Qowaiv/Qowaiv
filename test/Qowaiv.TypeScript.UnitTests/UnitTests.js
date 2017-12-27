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
          * @returns A GUID if valid, otherwise null.
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
            return /^[0-9ABCDEF]{32}$/i.test(s.replace(/-/g, ''));
        };
        /**
         * Creates a GUID.
         * @param {string} s A string containing GUID to convert or a number.
         * @returns A GUID if valid, otherwise null.
         */
        Guid.parse = function (s) {
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
        };
        /**
         * Returns a new empty GUID.
         */
        Guid.empty = function () {
            return new Guid();
        };
        /**
         * Creates a GUID.
         * @returns A random GUID.
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
});
