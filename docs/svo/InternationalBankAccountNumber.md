# International Bank Account Number (IBAN)
Represents an IBAN as specified in ISO 13616.

``` C#
var iban = InternationalBankAccountNumber.Parse("nl20ingb0001234567");

iban.Country; // Country.NL
iban.Length; // 18
iban.ToString("F"); // NL20 INGB 0001 2345 67
iban.ToString("H"); // NL20 INGB 0001 2345 67 (with non-breaking spaces).
iban.ToString("O"); // NL20XXXXXXXXXXX567 (obfuscated).
```

An overview with all supported countries and patterns can be found [here](../../IBAN.md).
