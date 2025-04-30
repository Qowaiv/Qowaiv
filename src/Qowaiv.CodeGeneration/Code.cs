namespace Qowaiv.CodeGeneration;

/// <summary>Represents code that can be written to a <see cref="CSharpWriter"/>.</summary>
public interface Code
{
    /// <summary>Writes its representation to the code file.</summary>
    void WriteTo(CSharpWriter writer);
}
