namespace Web.HTTP_reponse_status_specs;

public class Constants
{
    public static IEnumerable<FieldInfo> Fields
        => typeof(HttpReponseStatus).GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(field => field.FieldType == typeof(HttpReponseStatus));

    [TestCaseSource(nameof(Fields))]
    public void prefixes_code(FieldInfo field)
    {
        var status = (HttpReponseStatus)field.GetValue(null);
        field.Name.Should().StartWith($"N{status.Code}_");
    }

    [TestCaseSource(nameof(Fields))]
    public void name_reflacts_phrase(FieldInfo field)
    {
        var status = (HttpReponseStatus)field.GetValue(null);
        var reflaction = status.ReasonPhrase
            .Replace(' ', '_')
            .Replace('-', '_')
            .Replace("'", "")
            .ToUpperInvariant();

        field.Name.ToUpperInvariant().Should().EndWith(reflaction);
    }
}
public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.HttpResponseStatus.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.HttpResponseStatus.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.HttpResponseStatus.Equals(HttpReponseStatus.N402_Payment_Required).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.HttpResponseStatus.Equals(HttpReponseStatus.N418_Im_a_teapot).Should().BeTrue();

    [Test]
    public void equal_to_same_object()
      => Svo.HttpResponseStatus.Equals((object)HttpReponseStatus.N418_Im_a_teapot).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.HttpResponseStatus == HttpReponseStatus.N418_Im_a_teapot).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.HttpResponseStatus == HttpReponseStatus.N402_Payment_Required).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.HttpResponseStatus != HttpReponseStatus.N418_Im_a_teapot).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.HttpResponseStatus != HttpReponseStatus.N402_Payment_Required).Should().BeTrue();

    [Test]
    public void hash_code_is_not_zero_and_reproducable_for_not_empty()
    {
        var hash = Svo.HttpResponseStatus.GetHashCode();
        hash.Should().NotBe(0);
        Svo.HttpResponseStatus.GetHashCode().Should().Be(hash);
    }
}
