using GateOps.Application.GateOperations;
using GateOps.Domain.GateOperations;
using Microsoft.EntityFrameworkCore;

namespace GateOps.Infrastructure.Persistence;

public sealed class GateAppointmentRepository(GateOpsDbContext dbContext) : IGateAppointmentRepository
{
    public Task<GateAppointment?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        dbContext.GateAppointments.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IReadOnlyList<GateAppointment>> GetActiveByContainerAsync(ContainerNumber containerNumber, CancellationToken ct = default)
    {
        return await dbContext.GateAppointments
            .Where(a => a.ContainerNumber == containerNumber
                        && (a.Status == AppointmentStatus.Scheduled || a.Status == AppointmentStatus.CheckedIn))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<GateAppointment>> ListAsync(AppointmentStatus? status, CancellationToken ct = default)
    {
        var query = dbContext.GateAppointments.AsQueryable();
        if (status is not null) query = query.Where(a => a.Status == status);
        return await query.ToListAsync(ct);
    }

    public async Task AddAsync(GateAppointment appointment, CancellationToken ct = default) =>
        await dbContext.GateAppointments.AddAsync(appointment, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) => dbContext.SaveChangesAsync(ct);
}
