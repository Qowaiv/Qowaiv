namespace Qowaiv.DomainModel.Collections;

/// <summary>Represents an (else)-if state.</summary>
internal enum IfState
{
    /// <summary>False, next <see cref="Then"/> will not be added.</summary>
    False = 0,

    /// <summary>True, next <see cref="Then"/> will be added.</summary>
    True = 1,

    /// <summary>Done, no <see cref="Then"/> be added, in this branch.</summary>
    Done = 2,
}
