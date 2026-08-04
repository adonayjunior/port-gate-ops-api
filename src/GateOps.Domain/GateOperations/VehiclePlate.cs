using GateOps.Domain.Common;

namespace GateOps.Domain.GateOperations;

/// <summary>A vehicle license plate. Deliberately loose validation (non-empty, reasonable
/// length) rather than a country-specific format, since a gate system may serve trucks
/// registered in multiple jurisdictions.</summary>
public sealed class VehiclePlate : IEquatable<VehiclePlate>
{
    public string Value { get; }

    private VehiclePlate(string value) => Value = value;

    public static VehiclePlate Create(string rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
            throw new InvalidGateOperationException("Vehicle plate cannot be empty.");

        var normalized = rawValue.Trim().ToUpperInvariant();
        if (normalized.Length is < 5 or > 10)
            throw new InvalidGateOperationException($"'{normalized}' does not look like a valid plate (expected 5-10 characters).");

        return new VehiclePlate(normalized);
    }

    public bool Equals(VehiclePlate? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as VehiclePlate);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
