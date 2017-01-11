module Qowaiv {
    
    /**
     * Represents a Globally unique identifier (GUID).
     */
    export class Guid implements IEquatable, IFormattable, IJsonStringifyable {

        /**
         * @constructor
         * @remarks It is the default constructor, for creating an actual GUID
         *          you will normally use Guid.newGuid() or Guid.parse(string).
         */
        constructor() { }

        /**
         * The underlying value.
         */
        private v = '00000000-0000-0000-0000-000000000000';

        /** 
         * Returns a string that represents the current GUID.
         */
        public toString(): string {
            return this.v;
        }
        /** 
         * Returns a string that represents the current GUID.
         */
        public format(f?: string): string {
            switch (f) {
                case 'B': return '{' + this.v + '}';
                case 'b': return '{' + this.v.toLowerCase() + '}';
                case 'S': return this.v.replace(/-/g, '');
                case 's': return this.v.replace(/-/g, '').toLowerCase();
                case 'l': return this.v.toLowerCase();
                case 'U': case 'u': default: return this.v;
            }
        }
        /** 
         * Returns a JSON representation of the GUID.
         */
        public toJSON(): string {
            return this.v;
        }

        /**
         * Creates a GUID from a JSON string.
         * @param {string} s A JSON string representing the GUID.
         * @returns A GUID if valid, otherwise null.
         */
        public static fromJSON(s: string): Guid {
            return Guid.parse(s);
        }

        /**
         * Returns true if other is not null or undefined and a GUID
         * representing the same value, otherwise false.
         */
        public equals(other: any): boolean{
            return other !== null &&
                other !== undefined &&
                other instanceof (Guid) &&
                other.v === this.v;
        }

        /**
         * Returns true if the val represents valid GUID, otherwise false.
         * @param {string} s A string containing GUID.
         * @remarks This method calls create(). It's of no use, to call isValid(),
         *          to avoid a create() call.
         */
        public static isValid(s: string): boolean {
            return /^[0-9ABCDEF]{32}$/i.test(s.replace(/-/g, ''));
        }

        /**
         * Creates a GUID.
         * @param {string} s A string containing GUID to convert or a number.
         * @returns A GUID if valid, otherwise null.
         */
        public static parse(s: string): Guid {
            
            // an empty string should equal Guid.Empty.
            if (s === '') { return new Guid(); }
            
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
         * Creates a GUID.
         * @returns A random GUID.
         */
        public static newGuid(seed?: Guid): Guid {

            var guid = new Guid();
            guid.v = (
                Guid.rndGuid(false) +
                Guid.rndGuid(true) +
                Guid.rndGuid(true) +
                Guid.rndGuid(false)).toUpperCase();

            if (seed !== null && seed instanceof(Guid))
            {
                var lookup = '0123456789ABCDEF';
                var merged = '';
                for (var i = 0; i < 36; i++) {
                    var l = lookup.indexOf(seed.v.charAt(i));
                    var r = lookup.indexOf(guid.v.charAt(i));
                    merged += l === -1 || r === -1 ? guid.v.charAt(i) : lookup.charAt(l ^ r);
                }
                guid.v = merged;
            }
            return guid;
        }

        /**
         * Creates random GUID blocks.
         * @remarks called 4 times by Guid.newGuid().
         */
        private static rndGuid(s: boolean): string {
            var p = (Math.random().toString(16) + '000000000').substr(2, 8);
            return s ? '-' + p.substr(0, 4) + '-' + p.substr(4, 4) : p;
        }
    }
} 