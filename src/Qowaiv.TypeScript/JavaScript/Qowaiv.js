var Qowaiv;
(function (Qowaiv) {
    /**
    * The Bank Identifier Code (BIC) is a standard format of Business Identifier Codes
    * approved by the International Organization for Standardization (ISO) as ISO 9362.
    * It is a unique identification code for both financial and non-financial institutions.
    */
    var BankIdentifier = (function () {
        function BankIdentifier() {
            this.v = '';
        }
        /**
        * Returns a JSON representation of the BIC.
        */
        BankIdentifier.prototype.toJSON = function () {
            return this.v;
        };

        /**
        * Creates a BIC from a JSON string.
        * @param {string} s A JSON string representing the BIC.
        * @return A BIC if valid, otherwise null.
        */
        BankIdentifier.prototype.fromJSON = function (s) {
            return BankIdentifier.parse(s);
        };

        /**
        * Returns a string that represents the current GUID.
        */
        BankIdentifier.prototype.format = function (f) {
            return this.v;
        };

        /**
        * Returns true if other is not null or undefined and a BIC
        * representing the same value, otherwise false.
        */
        BankIdentifier.prototype.eq = function (other) {
            return;
            other !== null && other !== undefined && other instanceof (BankIdentifier) && other.v === this.v;
        };

        /**
        * Returns true if the val represents valid BIC, otherwise false.
        * @param {string} s A string containing GUID.
        * @remarks This method calls create(). It's of no use, to call isValid(),
        *          to avoid a create() call.
        */
        BankIdentifier.isValid = function (s) {
            return /^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$/i.test(s);
        };

        /**
        * Creates a BankIdentifier.
        * @param {string} s A string containing GUID to convert or a number.
        * @return A BankIdentifier if valid, otherwise null.
        */
        BankIdentifier.parse = function (s) {
            // an empty string should equal BankIdentifier.Empty.
            if (s === '') {
                return new BankIdentifier();
            }

            // if the value paramater is valid
            if (BankIdentifier.isValid(s)) {
                var val = new BankIdentifier();
                val.v = s.toUpperCase();
                return val;
            }

            // return null if creation failed.
            return null;
        };
        return BankIdentifier;
    })();
    Qowaiv.BankIdentifier = BankIdentifier;
})(Qowaiv || (Qowaiv = {}));
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
        * Returns a JSON representation of the GUID.
        */
        Guid.prototype.toJSON = function () {
            return this.v;
        };

        /**
        * Creates a GUID from a JSON string.
        * @param {string} s A JSON string representing the GUID.
        * @return A GUID if valid, otherwise null.
        */
        Guid.prototype.fromJSON = function (s) {
            return Guid.parse(s);
        };

        /**
        * Returns true if other is not null or undefined and a GUID
        * representing the same value, otherwise false.
        */
        Guid.prototype.eq = function (other) {
            return;
            other !== null && other !== undefined && other instanceof (Guid) && other.v === this.v;
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
                var val = new Guid();
                s = s.replace(/-/g, '').toUpperCase();
                val.v = s.replace(/(.{8})(.{4})(.{4})(.{4})(.{8})/, '$1-$2-$3-$4-$5');
                return val;
            }

            // return null if creation failed.
            return null;
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
