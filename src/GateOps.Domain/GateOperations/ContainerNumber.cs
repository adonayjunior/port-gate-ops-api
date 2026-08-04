using System.Text.RegularExpressions;
using GateOps.Domain.Common;

namespace GateOps.Domain.GateOperations;

/// <summary>
/// An ISO 6346 container identification number: 3-letter owner code + category
/// identifier (U/J/Z) + 6-digit serial number + 1 check digit (e.g. "MSCU1234567").
/// Immutable value object — equality is by value, not identity.
/// </summary>
public sealed partial class ContainerNumber : IEquatable<ContainerNumber>
{
    public string Value { get; }

    private ContainerNumber(string value) => Value = value;

    public static ContainerNumber Create(string rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
            throw new InvalidContainerNumberException(rawValue ?? string.Empty, "value is empty");

        var normalized = rawValue.Trim().ToUpperInvariant();

        if (!FormatRegex().IsMatch(normalized))
            throw new InvalidContainerNumberException(normalized, "expected format AAAU1234567 (3 letters + category letter + 7 digits)");

        var expectedCheckDigit = ComputeCheckDigit(normalized[..10]);
        var actualCheckDigit = normalized[10] - '0';
        if (expectedCheckDigit != actualCheckDigit)
            throw new InvalidContainerNumberException(normalized, $"check digit mismatch (expected {expectedCheckDigit}, got {actualCheckDigit})");

        return new ContainerNumber(normalized);
    }

    /// <summary>
    /// ISO 6346 check digit algorithm: each of the first 10 characters gets a
    /// numeric value (digits: face value; letters: 10..38 skipping multiples of 11),
    /// weighted by 2^position and summed; the check digit is (sum mod 11) mod 10.
    /// </summary>
    private static int ComputeCheckDigit(string first10Chars)
    {
        long weightedSum = 0;
        for (var position = 0; position < first10Chars.Length; position++)
        {
            var value = LetterOrDigitValue(first10Chars[position]);
            weightedSum += value * (1L << position);
        }
        return (int)(weightedSum % 11 % 10);
    }

    private static int LetterOrDigitValue(char c) => char.IsDigit(c) ? c - '0' : LetterValue(c);

    /// <summary>A=10, B=12, C=13 ... skipping every value that's a multiple of 11 (11, 22, 33).</summary>
    private static int LetterValue(char c)
    {
        var value = 10;
        for (var letter = 'A'; letter < c; letter++)
        {
            value++;
            if (value % 11 == 0) value++;
        }
        if (value % 11 == 0) value++;
        return value;
    }

    [GeneratedRegex("^[A-Z]{3}[UJZ][0-9]{7}$")]
    private static partial Regex FormatRegex();

    public bool Equals(ContainerNumber? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as ContainerNumber);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
