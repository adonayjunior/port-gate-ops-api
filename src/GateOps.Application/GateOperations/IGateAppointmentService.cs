using GateOps.Domain.GateOperations;

namespace GateOps.Application.GateOperations;

public interface IGateAppointmentService
{
    Task<GateAppointmentDto> ScheduleAsync(ScheduleAppointmentRequest request, CancellationToken ct = default);
    Task<GateAppointmentDto> CheckInAsync(Guid id, CheckInRequest request, DateTimeOffset atUtc, CancellationToken ct = default);
    Task<GateAppointmentDto> CompleteAsync(Guid id, DateTimeOffset atUtc, CancellationToken ct = default);
    Task<GateAppointmentDto> CancelAsync(Guid id, CancellationToken ct = default);
    Task<GateAppointmentDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<GateAppointmentDto>> ListAsync(AppointmentStatus? status, CancellationToken ct = default);
}
