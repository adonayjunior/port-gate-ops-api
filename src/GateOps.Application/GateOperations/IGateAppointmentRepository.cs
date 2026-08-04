using GateOps.Domain.GateOperations;

namespace GateOps.Application.GateOperations;

/// <summary>Persistence port for GateAppointment, defined by the application layer
/// (not the infrastructure layer) per the dependency inversion principle.</summary>
public interface IGateAppointmentRepository
{
    Task<GateAppointment?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Appointments for a container that are still "in play" (Scheduled or
    /// CheckedIn) — used to prevent scheduling conflicting overlapping visits.</summary>
    Task<IReadOnlyList<GateAppointment>> GetActiveByContainerAsync(ContainerNumber containerNumber, CancellationToken ct = default);

    Task<IReadOnlyList<GateAppointment>> ListAsync(AppointmentStatus? status, CancellationToken ct = default);

    Task AddAsync(GateAppointment appointment, CancellationToken ct = default);

    /// <summary>Persists changes made to entities already tracked (e.g. after calling
    /// a mutating domain method like CheckIn).</summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}
