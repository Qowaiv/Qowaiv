// If not specified otherwise, the resources are in USA English.
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]

// For optimal .NET support this is enabled in all assemblies.
[assembly: System.CLSCompliant(true)]

// No COM visibility.
// Classes which are exposed to COM must have a parameterless constructor. That
// conflicts with the approach of Qowaiv.
[assembly: System.Runtime.InteropServices.ComVisible(false)]

[module: System.Runtime.CompilerServices.SkipLocalsInit]
