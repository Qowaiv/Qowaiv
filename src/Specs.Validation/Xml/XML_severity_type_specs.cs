using Qowaiv.Validation.Abstractions;
using System.Xml.Schema;

namespace XML_severity_type_specs;

public class Maps
{
    [Test]
    public void warning_as_warning()
        => XmlSeverityType.Warning.ValidationSeverity.Should().Be(ValidationSeverity.Warning);

    [Test]
    [Arguments(XmlSeverityType.Error)]
    [Arguments((XmlSeverityType)666)]
    public void others_as_error(XmlSeverityType severityType)
      => severityType.ValidationSeverity.Should().Be(ValidationSeverity.Error);
}
