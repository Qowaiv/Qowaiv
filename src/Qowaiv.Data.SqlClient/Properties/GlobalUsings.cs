global using Qowaiv.Diagnostics.Contracts;
global using Qowaiv.Formatting;
global using Qowaiv.Hashing;
global using Qowaiv.OpenApi;
global using Qowaiv.Text;
global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.ComponentModel;
global using System.Data.SqlClient;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.Diagnostics.Contracts;
global using System.Globalization;
global using System.Linq;
global using System.Numerics;
global using System.Resources;
global using System.Runtime.Serialization;
global using System.Threading;
global using System.Xml;
global using System.Xml.Schema;
global using System.Xml.Serialization;

#if NET9_0_OR_GREATER
global using Lock = System.Threading.Lock;
#else
global using Lock = System.Object;
#endif
