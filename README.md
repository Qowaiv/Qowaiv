Qowaiv
======
Domain Driven Design bottom up
------------------------------

Qowaiv is a (Single) Value Object library. Its aims to model reusable (Single) Value Objects that can be used a wide variety of modelling scenarios, both inside and outsite a Domain Driven Design context.

Supported scenarios include parsing, formatting, validation, (de)serialization, and domain specific logic.

Qowaiv types
============

Bank Identifier Code (BIC)
----------------------------------------
Represents a BIC as specified in ISO 13616.

Country
-------
Represents a country based on an ISO 3166-1 code (or 3166-3 if the country does not longer exists).

Email address
-------------
Represents a (single) email address, including IPv4 domains.

Gender
------
Represents a gender based on an ISO 5218 code.

House number
------------
Represents a house number in the range [1-999999999].

International Bank Account Number (IBAN)
----------------------------------------
Represents an IBAN as specified in ISO 13616.

Month
-----
Represents a month in the range [1-12].

Percentage
----------
Represents a percentage/per mile/per ten thousend.

Postal code
-----------
Represents a postal code. It supports validation for all countries.

Year
----
Represents a year in the range [1-9999].
