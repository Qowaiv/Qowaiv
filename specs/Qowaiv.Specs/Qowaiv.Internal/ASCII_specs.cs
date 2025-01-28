namespace Text.ASCII_specs;

public class Email
{
    /// <remarks>
    /// Due to this internal code being included, a warning was produced on the
    /// code not being used. Now it is.
    /// </remarks>
    [Test]
    public void is_referenced()
        => ASCII.EmailAddress.IsLocal('q').Should().BeTrue();
}
