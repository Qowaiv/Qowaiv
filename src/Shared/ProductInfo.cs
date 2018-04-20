using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// If not specified otherwise, the resources are in USA English.
[assembly: NeutralResourcesLanguage("en-US")]

// For optimal .NET support this is enabled in all assemblies.
[assembly: CLSCompliant(true)]

// No COM visibility.
// Classes which are exposed to COM must have a parameterless constructor. That
// conflicts with the approach of Qowaiv.
[assembly: ComVisible(false)]
