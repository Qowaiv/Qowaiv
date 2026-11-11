# Email address
Represents a (single) email address. Supports:
* Display names (are stripped)
* Comments (are removed)
* IP-based domains (normalized and surrounded by brackets)

Furthermore, the email address is normalized as a lowercase string, making it
case-insensitive.

``` C#
var email = EmailAddress.Parse("Test Account <mailto:TEST@qowaiv.org>");
var quoted = EmailAddress.Parse("\"Joe Smith\" email@qowaiv.org");
var sld = EmailAddress.Parse("admin@localhost");
var ip_based = EmailAddress.Parse("test@[172.16.254.1]");

email.ToString(); // test@qowaiv.org
quoted.ToString(); // email@qowaiv.org
sld.HasSingleDomainLabel; // true
ip_based.IsIPBased; // true
ip_based.WithDisplayName("Jimi Hendrix"); // Jimi Hendrix <test@[172.16.254.1]>
```
