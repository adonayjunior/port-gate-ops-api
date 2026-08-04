namespace GateOps.Domain.Common;

public sealed class InvalidGateOperationException(string message) : DomainException(message);
