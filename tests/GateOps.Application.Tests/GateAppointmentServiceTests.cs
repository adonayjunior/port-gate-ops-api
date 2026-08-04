using GateOps.Application.GateOperations;
using GateOps.Domain.Common;
using GateOps.Domain.GateOperations;
using Xunit;

namespace GateOps.Application.Tests;

public class GateAppointmentServiceTests
{
    private static readonly DateTimeOffset WindowStart = new(2026, 1, 10, 9, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset WindowEnd = new(2026, 1, 10, 10, 0, 0, TimeSpan.Zero);

    private static ScheduleAppointmentRequest SampleRequest(string containerNumber = "CSQU3054383") => new(
        containerNumber, "ABC1D23", GateDirection.Inbound, WindowStart, WindowEnd);

    private static GateAppointmentService NewService(out FakeGateAppointmentRepository repository)
    {
        repository = new FakeGateAppointmentRepository();
        return new GateAppointmentService(repository);
    }

    [Fact]
    public async Task ScheduleAsync_PersistsANewAppointment()
    {
        var service = NewService(out _);

        var dto = await service.ScheduleAsync(SampleRequest());

        Assert.Equal(AppointmentStatus.Scheduled, dto.Status);
        Assert.Equal("CSQU3054383", dto.ContainerNumber);
    }

    [Fact]
    public async Task ScheduleAsync_WithAContainerThatAlreadyHasAnActiveAppointment_Throws()
    {
        var service = NewService(out _);
        await service.ScheduleAsync(SampleRequest());

        await Assert.ThrowsAsync<InvalidGateOperationException>(() => service.ScheduleAsync(SampleRequest()));
    }

    [Fact]
    public async Task ScheduleAsync_AfterThePreviousAppointmentIsCompleted_Succeeds()
    {
        var service = NewService(out _);
        var first = await service.ScheduleAsync(SampleRequest());
        await service.CheckInAsync(first.Id, new CheckInRequest("lane-1"), WindowStart);
        await service.CompleteAsync(first.Id, WindowStart.AddMinutes(10));

        var second = await service.ScheduleAsync(SampleRequest());

        Assert.Equal(AppointmentStatus.Scheduled, second.Status);
        Assert.NotEqual(first.Id, second.Id);
    }

    [Fact]
    public async Task CheckInAsync_ForAnUnknownAppointment_ThrowsNotFound()
    {
        var service = NewService(out _);

        await Assert.ThrowsAsync<AppointmentNotFoundException>(
            () => service.CheckInAsync(Guid.NewGuid(), new CheckInRequest("lane-1"), WindowStart));
    }

    [Fact]
    public async Task FullLifecycle_ScheduleCheckInComplete_ReportsExpectedStatusAtEachStep()
    {
        var service = NewService(out _);
        var scheduled = await service.ScheduleAsync(SampleRequest());
        Assert.Equal(AppointmentStatus.Scheduled, scheduled.Status);

        var checkedIn = await service.CheckInAsync(scheduled.Id, new CheckInRequest("lane-2"), WindowStart.AddMinutes(5));
        Assert.Equal(AppointmentStatus.CheckedIn, checkedIn.Status);
        Assert.Equal("LANE-2", checkedIn.GateLane);

        var completed = await service.CompleteAsync(scheduled.Id, WindowStart.AddMinutes(25));
        Assert.Equal(AppointmentStatus.Completed, completed.Status);
    }

    [Fact]
    public async Task CancelAsync_AScheduledAppointment_Succeeds()
    {
        var service = NewService(out _);
        var scheduled = await service.ScheduleAsync(SampleRequest());

        var cancelled = await service.CancelAsync(scheduled.Id);

        Assert.Equal(AppointmentStatus.Cancelled, cancelled.Status);
    }

    [Fact]
    public async Task ListAsync_FiltersByStatus()
    {
        var service = NewService(out _);
        var scheduled = await service.ScheduleAsync(SampleRequest());
        var other = await service.ScheduleAsync(SampleRequest("MSCU1234566"));
        await service.CancelAsync(other.Id);

        var onlyScheduled = await service.ListAsync(AppointmentStatus.Scheduled);

        Assert.Single(onlyScheduled);
        Assert.Equal(scheduled.Id, onlyScheduled[0].Id);
    }
}
