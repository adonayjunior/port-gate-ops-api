namespace GateOps.Application.GateOperations;

public sealed class AppointmentNotFoundException(Guid id) : Exception($"Gate appointment '{id}' was not found.");
