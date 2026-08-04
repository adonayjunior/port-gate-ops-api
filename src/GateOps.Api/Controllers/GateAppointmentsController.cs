using GateOps.Application.GateOperations;
using GateOps.Domain.GateOperations;
using Microsoft.AspNetCore.Mvc;

namespace GateOps.Api.Controllers;

[ApiController]
[Route("api/gate-appointments")]
public sealed class GateAppointmentsController(IGateAppointmentService service) : ControllerBase
{
    /// <summary>Schedules a new gate visit for a container.</summary>
    [HttpPost]
    [ProducesResponseType<GateAppointmentDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<GateAppointmentDto>> Schedule(ScheduleAppointmentRequest request, CancellationToken ct)
    {
        var dto = await service.ScheduleAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<GateAppointmentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GateAppointmentDto>> GetById(Guid id, CancellationToken ct)
    {
        var dto = await service.GetByIdAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<GateAppointmentDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<GateAppointmentDto>>> List([FromQuery] AppointmentStatus? status, CancellationToken ct)
    {
        return Ok(await service.ListAsync(status, ct));
    }

    /// <summary>Registers the vehicle's arrival at the gate.</summary>
    [HttpPost("{id:guid}/check-in")]
    [ProducesResponseType<GateAppointmentDto>(StatusCodes.Status200OK)]
    public async Task<ActionResult<GateAppointmentDto>> CheckIn(Guid id, CheckInRequest request, CancellationToken ct)
    {
        return Ok(await service.CheckInAsync(id, request, DateTimeOffset.UtcNow, ct));
    }

    /// <summary>Marks the gate movement as physically completed (container moved in/out).</summary>
    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType<GateAppointmentDto>(StatusCodes.Status200OK)]
    public async Task<ActionResult<GateAppointmentDto>> Complete(Guid id, CancellationToken ct)
    {
        return Ok(await service.CompleteAsync(id, DateTimeOffset.UtcNow, ct));
    }

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType<GateAppointmentDto>(StatusCodes.Status200OK)]
    public async Task<ActionResult<GateAppointmentDto>> Cancel(Guid id, CancellationToken ct)
    {
        return Ok(await service.CancelAsync(id, ct));
    }
}
