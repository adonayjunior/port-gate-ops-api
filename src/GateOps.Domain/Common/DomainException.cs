namespace GateOps.Domain.Common;

/// <summary>Base type for violations of a domain invariant (as opposed to infrastructure/validation errors).</summary>
public abstract class DomainException(string message) : Exception(message);
