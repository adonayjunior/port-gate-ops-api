using GateOps.Application.GateOperations;
using GateOps.Domain.GateOperations;

namespace GateOps.Application.Tests;

/// <summary>An in-memory stand-in for the repository, so Application tests don't
/// need Infrastructure/EF Core — keeps the dependency direction clean and the tests fast.</summary>
public sealed class FakeGateAppointmentRepository : IGateAppointmentRepository
{
    private readonly Dictionary<Guid, GateAppointment> _appointments = new();

    public Task<GateAppointment?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(_appointments.GetValueOrDefault(id));

    public Task<IReadOnlyList<GateAppointment>> GetActiveByContainerAsync(ContainerNumber containerNumber, CancellationToken ct = default)
    {
        IReadOnlyList<GateAppointment> active = _appointments.Values
            .Where(a => a.ContainerNumber.Equals(containerNumber)
                        && a.Status is AppointmentStatus.Scheduled or AppointmentStatus.CheckedIn)
            .ToList();
        return Task.FromResult(active);
    }

    public Task<IReadOnlyList<GateAppointment>> ListAsync(AppointmentStatus? status, CancellationToken ct = default)
    {
        IReadOnlyList<GateAppointment> results = _appointments.Values
            .Where(a => status is null || a.Status == status)
            .ToList();
        return Task.FromResult(results);
    }

    public Task AddAsync(GateAppointment appointment, CancellationToken ct = default)
    {
        _appointments[appointment.Id] = appointment;
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
}
