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
        BankIdentifier.fromJSON = function (s) {
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
        BankIdentifier.prototype.equals = function (other) {
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

            // if the value parameter is valid
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
    /**
    * Represents a Globally unique identifier (GUID).
    */
    var Guid = (function () {
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
                case 'B':
                    return '{' + this.v + '}';
                case 'b':
                    return '{' + this.v.toLowerCase() + '}';
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
        Guid.fromJSON = function (s) {
            return Guid.parse(s);
        };

        /**
        * Returns true if other is not null or undefined and a GUID
        * representing the same value, otherwise false.
        */
        Guid.prototype.equals = function (other) {
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
var Qowaiv;
(function (Qowaiv) {
    /**
    * Represents a (second based) time span.
    */
    var TimeSpan = (function () {
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
        * Returns the total of ticks of the time span.
        */
        TimeSpan.prototype.getTicks = function () {
            return ~~(this.v * 10000000);
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
        * @return TimeSpan if valid, otherwise null.
        */
        TimeSpan.fromJSON = function (s) {
            return TimeSpan.parse(s);
        };

        /**
        * Returns true if other is not null or undefined and a TimeSpan
        * representing the same value, otherwise false.
        */
        TimeSpan.prototype.equals = function (other) {
            return;
            other !== null && other !== undefined && other instanceof (TimeSpan) && other.v === this.v;
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
        * @return A TimeSpan if valid, otherwise null.
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
        TimeSpan.pattern = /^\d*((((\d+:(2[0-3]|[0-1]\d)|\d)?:[0-5])?\d:)?[0-5])?\d([,\.]\d+)?$/;
        return TimeSpan;
    })();
    Qowaiv.TimeSpan = TimeSpan;
})(Qowaiv || (Qowaiv = {}));
//# sourceMappingURL=Qowaiv.js.map
