using GateOps.Domain.GateOperations;

namespace GateOps.Application.GateOperations;

public sealed record GateAppointmentDto(
    Guid Id,
    string ContainerNumber,
    string VehiclePlate,
    GateDirection Direction,
    DateTimeOffset ScheduledWindowStart,
    DateTimeOffset ScheduledWindowEnd,
    AppointmentStatus Status,
    DateTimeOffset? CheckedInAtUtc,
    DateTimeOffset? CompletedAtUtc,
    string? GateLane)
{
    public static GateAppointmentDto From(GateAppointment appointment) => new(
        appointment.Id,
        appointment.ContainerNumber.Value,
        appointment.VehiclePlate.Value,
        appointment.Direction,
        appointment.ScheduledWindowStart,
        appointment.ScheduledWindowEnd,
        appointment.Status,
        appointment.CheckedInAtUtc,
        appointment.CompletedAtUtc,
        appointment.GateLane);
}

public sealed record ScheduleAppointmentRequest(
    string ContainerNumber,
    string VehiclePlate,
    GateDirection Direction,
    DateTimeOffset ScheduledWindowStart,
    DateTimeOffset ScheduledWindowEnd);

public sealed record CheckInRequest(string GateLane);
