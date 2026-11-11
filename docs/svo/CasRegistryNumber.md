# CAS Registry Number
A CAS Registry Number, is a unique numerical identifier assigned by the
Chemical Abstracts Service (CAS), US to every chemical substance described
in the open scientific literature. It includes all substances described
from 1957 through the present, plus some substances from as far back as the
early 1800's.

``` C#
// Creation
var fromString = CasRegistryNumber.Parse("7732-18-5");
var fromInt32 = 7732_18_5.CasNr();
var fromInt64 = 7732_18_5L.CasNr();
```
