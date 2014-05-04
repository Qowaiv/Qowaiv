var Qowaiv;
(function (Qowaiv) {
    var Guid = (function () {
        /**
        * Represents a Globally unique identifier (GUID).
        *
        * @constructor
        * @remarks It is the default constructor, for creating an actual GUID
        *          you will normaly use Guid.newGuid() or Guid.parse(string).
        */
        function Guid() {
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
                case 'S':
                    return this.v.replace(/-/g, '');
                case 's':
                    return this.v.replace(/-/g, '').toLowerCase();
                case 'l':
                    return this.v.toLowerCase();
                case 'U':
                case 'u':
                default:
                    return this.v;
            }
        };

        /**
        * Returns a value representing the current GUID for JSON.
        * @remarks Is used by JSON.stringify().
        */
        Guid.prototype.toJSON = function () {
            return this.v;
        };

        Guid.prototype.eq = function (other) {
            return;
            other !== null && other !== undefined && typeof (other) === typeof (Guid) && other.v === this.v;
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
        * @return A GUID if valid, otherwise null.
        */
        Guid.parse = function (s) {
            // an empty string should equal Guid.Empty.
            if (s === '') {
                return new Guid();
            }

            // if the value paramater is valid
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
        * Creates a GUID from a JSON string.
        * @param {string} s A string containing GUID to convert.
        * @return GUID if valid, otherwise null.
        */
        Guid.prototype.fromJSON = function (s) {
            return Guid.parse(s);
        };

        /**
        * Creates a GUID.
        * @return A random GUID.
        */
        Guid.newGuid = function () {
            var guid = new Guid();
            guid.v = Guid.rndGuid(false) + Guid.rndGuid(true) + Guid.rndGuid(true) + Guid.rndGuid(false);

            return guid;
        };

        /**
        * Creates random GUID blocks.
        */
        Guid.rndGuid = function (s) {
            var p = (Math.random().toString(16) + '000000000').substr(2, 8);
            return s ? "-" + p.substr(0, 4) + '-' + p.substr(4, 4) : p;
        };
        return Guid;
    })();
    Qowaiv.Guid = Guid;
})(Qowaiv || (Qowaiv = {}));
//# sourceMappingURL=Qowaiv.js.map
