Qowaiv
======
Domain-driven design bottom up
------------------------------

Qowaiv is a (Single) Value Object library. It aims to model reusable (Single)
Value Objects that can be used a wide variety of modeling scenarios, both
inside and outside a Domain-driven context.

Supported scenarios include parsing, formatting, validation, (de)serialization,
and domain specific logic.

Qowaiv types
============

Bank Identifier Code (BIC)
--------------------------
Represents a BIC as specified in ISO 13616.

Country
-------
Represents a country based on an ISO 3166-1 code (or 3166-3 if the country does not longer exists).

Date
----
Represents a date, so without hours (minutes, seconds, milliseconds).

Elo
---
Represents an Elo (rating), a method for calculating the relative skill levels of
players in competitor-versus-competitor games.

Email address
-------------
Represents a (single) email address, including IPv4 domains.

Email address collection
------------------------
Represents a collection of unique email addresses, excluding the empty and unknown email address.

Stream size
---------
Represents the size of a file or stream.

Gender
------
Represents a gender based on an ISO 5218 code.

House number
------------
Represents a house number in the range [1-999999999].

International Bank Account Number (IBAN)
----------------------------------------
Represents an IBAN as specified in ISO 13616.

Local Date Time
---------------
Explicitly marked local date time. It allows the clear distinction between local 
and UTC-based date times.

Month
-----
Represents a month in the range [1-12].

Percentage
----------
Represents a percentage/per mile/per ten thousand.

Postal code
-----------
Represents a postal code. It supports validation for all countries.

UUID aka GUID
-------------
The UUID (Universally unique identifier) aka GUID (Globally unique identifier) is an
extension on the System.Guid. It is by default represented by a 22 length string, 
instead of a 32 length string.

Week date
---------
Represents a week based date.

Year
----
Represents a year in the range [1-9999].

Qowaiv cryptographical types
============================ 

Seed
----
A seed, representing random data to encrypt and decrypt data.

Qowaiv financial types
======================

Currency
--------
Represents a currency based on an ISO 4217 code.

Qowaiv SQL types
================

Timestamp
---------
Represents a (MS SQL) timestamp is a data type that exposes automatically generated
binary numbers, which are guaranteed to be unique within a database. timestamp is
used typically as a mechanism for version-stamping table rows. The storage size is
8 bytes. See: https://technet.microsoft.com/en-us/library/aa260631%28v=sql.80%29.aspx

Qowaiv Data
===========

SVO Parameter factory class
---------------------------
To create a (SQL) parameter with a SVO as value, use the SvoParamater factory
class. It will return SQL parameter with a converted database proof value.

Qowaiv web types
================

Internet media type
-------------------
Represents a internet media type (also known as MIME-type and content type).

Qowaiv complex types
====================

Wildcard pattern
----------------
Represents a pattern to match strings, using wildcard characters ? and *. It 
also support the use of SQL wildcard characters _ and %.

Qowaiv helpers
==============

Guard
-----
Guard parameters, for centralizing and simplifying the argument checking.

Qowaiv SVO options
==================

Debugger display
----------------
During debugging sessions, by default, the IDE shows the result of ToString()
on a watch. Although Tostring() is overridden for all Qowaiv Single Value 
Objects, for debugging a special debugger display is provided too, using a 
debugger display attribute.

The debugger display attribute refers to (private) property with the name 
"DebuggerDisplay", which represents the Single Value Object as string. If 
supported, formatted, and in case of a Empty or Unknown value with a 
notification of that too. The outcome of the DebuggerDisplay is tested in the 
UnitTests.

Because the rendering of debugger display is handled based on the development 
environment, and methods as debugger display are not supported by VB.NET, the 
debugger display attribute refers to a property instead.

Qowaiv Formatting
=================
Formatting is an important part of the functionality in Qowaiv. All SVO's 
implement IFormattable, and have custom formatting. For details, see the 
different remarks at the ToString(string, IFormatProvider).

Formatting arguments
--------------------
The formatting arguments object, is a container object (struct) of the format 
and the format provider, the two arguments required for the System.Iformatable 
ToString() method.

Formatting arguments collection
-------------------------------
This collection of formatting arguments stores them based on a type to apply 
on. On top of that, it has a Format() method, that is an extended implementation 
of string.Format(). The difference between these two methods is, that - when no 
custom format is supplied at the format string - string.Format() the default 
formatting of the object is used, where FormattingArgumentsCollection.Format() 
uses the default specified at the formatting collection of a type (if available).

Threading
=========
Because there are scenario's where you want to set typical values as a country 
or a currency for the context of the current thread (like the culture info) 
there is a possibility to add these to the Qowaiv.Threading.TrheadDomain.

These values can be configured (in the app settings) or can be created with a 
creator function that can be registered. If not specified otherwise the current 
country will be created (if possible) based on the current culture.
