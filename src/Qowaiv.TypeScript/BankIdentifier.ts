module Qowaiv {

    /**
     * The Bank Identifier Code (BIC) is a standard format of Business Identifier Codes
     * approved by the International Organization for Standardization (ISO) as ISO 9362.
     * It is a unique identification code for both financial and non-financial institutions.
     */
    export class BankIdentifier implements IEquatable, IFormattable, IJsonStringifyable {

        constructor() {
            this.v = '';
        }

        /**
         * The underlying value.
         */
        private v: string;

        /** 
         * Returns a JSON representation of the BIC.
         */
        toJSON(): string {
            return this.v;
        }
        
        /**
         * Creates a BIC from a JSON string.
         * @param {string} s A JSON string representing the BIC.
         * @return A BIC if valid, otherwise null.
         */
        public fromJSON(s: string) {
            return BankIdentifier.parse(s);
        }

        /** 
         * Returns a string that represents the current GUID.
         */
        public format(f?: string): string {
            return this.v;
        }

        /**
         * Returns true if other is not null or undefined and a BIC
         * representing the same value, otherwise false.
         */
        public eq(other: any): boolean {
            return
                other !== null &&
                other !== undefined &&
                other instanceof (BankIdentifier) &&
                other.v === this.v;
        }

        /**
         * Returns true if the val represents valid BIC, otherwise false.
         * @param {string} s A string containing GUID.
         * @remarks This method calls create(). It's of no use, to call isValid(),
         *          to avoid a create() call.
         */
        public static isValid(s: string): boolean {
            return /^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$/i.test(s);
        }

        /**
         * Creates a BankIdentifier.
         * @param {string} s A string containing GUID to convert or a number.
         * @return A BankIdentifier if valid, otherwise null.
         */
        public static parse(s: string): BankIdentifier {

            // an empty string should equal BankIdentifier.Empty.
            if (s === '') { return new BankIdentifier(); }

            // if the value paramater is valid
            if (BankIdentifier.isValid(s)) {
                var val = new BankIdentifier();
                val.v = s.toUpperCase();
                return val;
            }

            // return null if creation failed.
            return null;
        }
	}
}