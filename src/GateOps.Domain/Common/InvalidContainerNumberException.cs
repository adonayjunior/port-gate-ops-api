namespace GateOps.Domain.Common;

public sealed class InvalidContainerNumberException(string value, string reason)
    : DomainException($"'{value}' is not a valid ISO 6346 container number: {reason}");
