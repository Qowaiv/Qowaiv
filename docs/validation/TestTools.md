# Qowaiv.Validation Test Tools

These test tools help to write meaningful tests and assert the state of a
`Result` or `Result<T>`. The syntax is inspired by [FluentAssertions](https://fluentassertions.com).

These assertions could look like this:

``` C#
Result.For(17).Should().BeValid()
    .WithoutMessages()
    .Value.Should().Be(17); 
```
Where the chain after `.Value` exposes the value.

Other options for `.BeValid()` are:

``` C#
Result.For(17).Should().BeValid()
    .WithMessage(ValidationMessage.Warn("Are you sure?"))
    .Value.Should().Be(17);
```
and

``` C#
Result.For(17).Should().BeValid()
    .WithMessages(/* ... */)
    .Value.Should().Be(17);
```

Obviously, you can also check for invalidity:

``` C#
Result.For(17).Should().BeInvalid(); 
```

In that case you do not have the option of `.WithoutMessages()`, obviously,
as `Result` is defined to be invalid based on containing at least one error
message. Furthermore, it does not expose the `.And.Value` as that is not
accessible for an invalid result.

Everything except `.And.Value` also applies to the non-generic `Result`.

## Validate with
There is also some syntactic sugar for validating a model of choice:

``` C#
var model = new MyModel();

Result<MyModel> result = model.ValidateWith(new ValidatorForMyModel());
```
And this result can be changed with the assertions described on top:

``` C#
var model = new MyModel();
model.ValidateWith(new ValidatorForMyModel()).Should().BeValid();
```
