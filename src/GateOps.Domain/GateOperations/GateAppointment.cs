using GateOps.Domain.Common;

namespace GateOps.Domain.GateOperations;

/// <summary>
/// A scheduled gate visit for a container/vehicle. This is the aggregate root for
/// gate operations: it owns the lifecycle (Scheduled -> CheckedIn -> Completed, or
/// Cancelled/Expired) and enforces the invariants around when each transition is
/// allowed. A grace window is applied around the scheduled slot to reflect how real
/// gate operations work (trucks are rarely exactly on time).
/// </summary>
public sealed class GateAppointment
{
    private static readonly TimeSpan GraceWindow = TimeSpan.FromMinutes(30);

    public Guid Id { get; private set; }
    public ContainerNumber ContainerNumber { get; private set; } = null!;
    public VehiclePlate VehiclePlate { get; private set; } = null!;
    public GateDirection Direction { get; private set; }
    public DateTimeOffset ScheduledWindowStart { get; private set; }
    public DateTimeOffset ScheduledWindowEnd { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public DateTimeOffset? CheckedInAtUtc { get; private set; }
    public DateTimeOffset? CompletedAtUtc { get; private set; }
    public string? GateLane { get; private set; }

    private GateAppointment(
        Guid id,
        ContainerNumber containerNumber,
        VehiclePlate vehiclePlate,
        GateDirection direction,
        DateTimeOffset scheduledWindowStart,
        DateTimeOffset scheduledWindowEnd)
    {
        Id = id;
        ContainerNumber = containerNumber;
        VehiclePlate = vehiclePlate;
        Direction = direction;
        ScheduledWindowStart = scheduledWindowStart;
        ScheduledWindowEnd = scheduledWindowEnd;
        Status = AppointmentStatus.Scheduled;
    }

    // Parameterless constructor for EF Core materialization.
    private GateAppointment() { }

    public static GateAppointment Schedule(
        ContainerNumber containerNumber,
        VehiclePlate vehiclePlate,
        GateDirection direction,
        DateTimeOffset scheduledWindowStart,
        DateTimeOffset scheduledWindowEnd)
    {
        if (scheduledWindowEnd <= scheduledWindowStart)
            throw new InvalidGateOperationException("The scheduled window end must be after its start.");

        return new GateAppointment(
            Guid.NewGuid(), containerNumber, vehiclePlate, direction, scheduledWindowStart, scheduledWindowEnd);
    }

    /// <summary>Registers the vehicle's arrival at the gate, assigning it to a lane.
    /// Only allowed while Scheduled and within the scheduled window (± grace period).</summary>
    public void CheckIn(DateTimeOffset atUtc, string gateLane)
    {
        if (Status != AppointmentStatus.Scheduled)
            throw new InvalidGateOperationException($"Cannot check in an appointment in status '{Status}'.");

        if (string.IsNullOrWhiteSpace(gateLane))
            throw new InvalidGateOperationException("A gate lane must be assigned at check-in.");

        if (atUtc < ScheduledWindowStart - GraceWindow || atUtc > ScheduledWindowEnd + GraceWindow)
            throw new InvalidGateOperationException(
                $"Check-in at {atUtc:O} is outside the allowed window ({ScheduledWindowStart:O} - {ScheduledWindowEnd:O}, ±{GraceWindow.TotalMinutes}min grace).");

        Status = AppointmentStatus.CheckedIn;
        CheckedInAtUtc = atUtc;
        GateLane = gateLane.Trim().ToUpperInvariant();
    }

    /// <summary>Marks the gate operation as finished (container physically moved in/out).
    /// Only allowed after check-in.</summary>
    public void Complete(DateTimeOffset atUtc)
    {
        if (Status != AppointmentStatus.CheckedIn)
            throw new InvalidGateOperationException($"Cannot complete an appointment in status '{Status}'; it must be checked in first.");

        if (atUtc < CheckedInAtUtc)
            throw new InvalidGateOperationException("Completion time cannot be before check-in time.");

        Status = AppointmentStatus.Completed;
        CompletedAtUtc = atUtc;
    }

    /// <summary>Cancels a not-yet-started appointment.</summary>
    public void Cancel()
    {
        if (Status != AppointmentStatus.Scheduled)
            throw new InvalidGateOperationException($"Cannot cancel an appointment in status '{Status}'.");

        Status = AppointmentStatus.Cancelled;
    }

    /// <summary>Marks the appointment as expired if the scheduled window (+ grace) has
    /// elapsed without a check-in. This is idempotent and a no-op for other statuses.</summary>
    public void ExpireIfOverdue(DateTimeOffset atUtc)
    {
        if (Status == AppointmentStatus.Scheduled && atUtc > ScheduledWindowEnd + GraceWindow)
            Status = AppointmentStatus.Expired;
    }
}
