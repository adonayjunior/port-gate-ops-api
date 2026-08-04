using GateOps.Domain.Common;
using GateOps.Domain.GateOperations;

namespace GateOps.Application.GateOperations;

public sealed class GateAppointmentService(IGateAppointmentRepository repository) : IGateAppointmentService
{
    public async Task<GateAppointmentDto> ScheduleAsync(ScheduleAppointmentRequest request, CancellationToken ct = default)
    {
        var containerNumber = ContainerNumber.Create(request.ContainerNumber);
        var vehiclePlate = VehiclePlate.Create(request.VehiclePlate);

        var activeForContainer = await repository.GetActiveByContainerAsync(containerNumber, ct);
        if (activeForContainer.Count > 0)
            throw new InvalidGateOperationException(
                $"Container '{containerNumber}' already has an active gate appointment ({activeForContainer[0].Id}); complete or cancel it first.");

        var appointment = GateAppointment.Schedule(
            containerNumber, vehiclePlate, request.Direction, request.ScheduledWindowStart, request.ScheduledWindowEnd);

        await repository.AddAsync(appointment, ct);
        await repository.SaveChangesAsync(ct);

        return GateAppointmentDto.From(appointment);
    }

    public async Task<GateAppointmentDto> CheckInAsync(Guid id, CheckInRequest request, DateTimeOffset atUtc, CancellationToken ct = default)
    {
        var appointment = await RequireAppointmentAsync(id, ct);
        appointment.CheckIn(atUtc, request.GateLane);
        await repository.SaveChangesAsync(ct);
        return GateAppointmentDto.From(appointment);
    }

    public async Task<GateAppointmentDto> CompleteAsync(Guid id, DateTimeOffset atUtc, CancellationToken ct = default)
    {
        var appointment = await RequireAppointmentAsync(id, ct);
        appointment.Complete(atUtc);
        await repository.SaveChangesAsync(ct);
        return GateAppointmentDto.From(appointment);
    }

    public async Task<GateAppointmentDto> CancelAsync(Guid id, CancellationToken ct = default)
    {
        var appointment = await RequireAppointmentAsync(id, ct);
        appointment.Cancel();
        await repository.SaveChangesAsync(ct);
        return GateAppointmentDto.From(appointment);
    }

    public async Task<GateAppointmentDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var appointment = await repository.GetByIdAsync(id, ct);
        return appointment is null ? null : GateAppointmentDto.From(appointment);
    }

    public async Task<IReadOnlyList<GateAppointmentDto>> ListAsync(AppointmentStatus? status, CancellationToken ct = default)
    {
        var appointments = await repository.ListAsync(status, ct);
        return appointments.Select(GateAppointmentDto.From).ToList();
    }

    private async Task<GateAppointment> RequireAppointmentAsync(Guid id, CancellationToken ct)
        => await repository.GetByIdAsync(id, ct) ?? throw new AppointmentNotFoundException(id);
}
