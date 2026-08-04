using GateOps.Domain.Common;
using GateOps.Domain.GateOperations;
using Xunit;

namespace GateOps.Domain.Tests;

public class ContainerNumberTests
{
    // CSQU3054383 is the worked example from the ISO 6346 standard's check-digit
    // explanation: check digit 3 for owner/serial "CSQU305438".
    [Theory]
    [InlineData("CSQU3054383")]
    [InlineData("csqu3054383")] // lowercase should be normalized
    [InlineData(" CSQU3054383 ")] // surrounding whitespace should be trimmed
    public void Create_AcceptsAValidNumber(string raw)
    {
        var number = ContainerNumber.Create(raw);
        Assert.Equal("CSQU3054383", number.Value);
    }

    [Fact]
    public void Create_RejectsAWrongCheckDigit()
    {
        Assert.Throws<InvalidContainerNumberException>(() => ContainerNumber.Create("CSQU3054380"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("TOO-SHORT")]
    [InlineData("1234U1234567")] // owner code must be letters
    [InlineData("CSQX30543835")] // wrong category letter and one digit too many
    public void Create_RejectsMalformedInput(string raw)
    {
        Assert.Throws<InvalidContainerNumberException>(() => ContainerNumber.Create(raw));
    }

    [Fact]
    public void TwoInstancesWithTheSameValue_AreEqual()
    {
        var a = ContainerNumber.Create("CSQU3054383");
        var b = ContainerNumber.Create("csqu3054383");
        Assert.Equal(a, b);
        Assert.True(a.Equals(b));
    }
}
