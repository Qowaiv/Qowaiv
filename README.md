Qowaiv
======
Domain-driven design bottom up
------------------------------

Qowaiv is a (Single) Value Object library. It aims to model reusable (Single) Value Objects that can be used a wide variety of modelling scenarios, both inside and outside a Domain-driven context.

Supported scenarios include parsing, formatting, validation, (de)serialization, and domain specific logic.

Qowaiv types
============

Bank Identifier Code (BIC)
----------------------------------------
Represents a BIC as specified in ISO 13616.

Country
-------
Represents a country based on an ISO 3166-1 code (or 3166-3 if the country does not longer exists).

Date
----
Represents a date, so without hours (minutes, seconds, mili seconds).

Email address
-------------
Represents a (single) email address, including IPv4 domains.

File size
---------
Represents the size of a file.

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

Week date
---------
Represents a week based date.

Year
----
Represents a year in the range [1-9999].

Qowaiv SVO options
==================

Debugger display
----------------
During debugging sessions, by default, the IDE shows the result of ToString() on a watch. Although Tostring() is overridden for all Qowaiv Single Value Objects, for debugging a special debugger display is provided too, using a debugger display attribute.

The debugger display attribute refers to (private) property with the name "DebuggerDisplay", which repersents the Single Value Object as string. If supported, formatted, and in case of a Empty or Unknown value with a notification of that too. The outcome of the DebuggerDisplay is tested in de UnitTests.

Because the rendering of debugger display is handled based on the development enviroment, and methods as debugger display are not supported by VB.NET, the debugger display attribute refers to a property instead.