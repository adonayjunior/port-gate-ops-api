using GateOps.Domain.Common;
using GateOps.Domain.GateOperations;
using Xunit;

namespace GateOps.Domain.Tests;

public class GateAppointmentTests
{
    private static readonly ContainerNumber SampleContainer = ContainerNumber.Create("CSQU3054383");
    private static readonly VehiclePlate SamplePlate = VehiclePlate.Create("ABC1D23");
    private static readonly DateTimeOffset WindowStart = new(2026, 1, 10, 9, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset WindowEnd = new(2026, 1, 10, 10, 0, 0, TimeSpan.Zero);

    private static GateAppointment NewScheduledAppointment() =>
        GateAppointment.Schedule(SampleContainer, SamplePlate, GateDirection.Inbound, WindowStart, WindowEnd);

    [Fact]
    public void Schedule_RejectsAWindowThatEndsBeforeItStarts()
    {
        Assert.Throws<InvalidGateOperationException>(() =>
            GateAppointment.Schedule(SampleContainer, SamplePlate, GateDirection.Inbound, WindowEnd, WindowStart));
    }

    [Fact]
    public void CheckIn_WithinTheScheduledWindow_Succeeds()
    {
        var appointment = NewScheduledAppointment();

        appointment.CheckIn(WindowStart.AddMinutes(15), "lane-3");

        Assert.Equal(AppointmentStatus.CheckedIn, appointment.Status);
        Assert.Equal("LANE-3", appointment.GateLane);
    }

    [Fact]
    public void CheckIn_WithinTheGracePeriodBeforeTheWindow_Succeeds()
    {
        var appointment = NewScheduledAppointment();

        appointment.CheckIn(WindowStart.AddMinutes(-20), "lane-1");

        Assert.Equal(AppointmentStatus.CheckedIn, appointment.Status);
    }

    [Fact]
    public void CheckIn_FarOutsideTheWindow_IsRejected()
    {
        var appointment = NewScheduledAppointment();

        var ex = Assert.Throws<InvalidGateOperationException>(() =>
            appointment.CheckIn(WindowStart.AddHours(-3), "lane-1"));
        Assert.Contains("outside the allowed window", ex.Message);
    }

    [Fact]
    public void CheckIn_Twice_IsRejected()
    {
        var appointment = NewScheduledAppointment();
        appointment.CheckIn(WindowStart, "lane-1");

        Assert.Throws<InvalidGateOperationException>(() => appointment.CheckIn(WindowStart.AddMinutes(5), "lane-2"));
    }

    [Fact]
    public void Complete_BeforeCheckIn_IsRejected()
    {
        var appointment = NewScheduledAppointment();

        Assert.Throws<InvalidGateOperationException>(() => appointment.Complete(WindowStart));
    }

    [Fact]
    public void Complete_AfterCheckIn_Succeeds()
    {
        var appointment = NewScheduledAppointment();
        appointment.CheckIn(WindowStart, "lane-1");

        appointment.Complete(WindowStart.AddMinutes(20));

        Assert.Equal(AppointmentStatus.Completed, appointment.Status);
        Assert.Equal(WindowStart.AddMinutes(20), appointment.CompletedAtUtc);
    }

    [Fact]
    public void Cancel_AScheduledAppointment_Succeeds()
    {
        var appointment = NewScheduledAppointment();

        appointment.Cancel();

        Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
    }

    [Fact]
    public void Cancel_AfterCheckIn_IsRejected()
    {
        var appointment = NewScheduledAppointment();
        appointment.CheckIn(WindowStart, "lane-1");

        Assert.Throws<InvalidGateOperationException>(() => appointment.Cancel());
    }

    [Fact]
    public void ExpireIfOverdue_PastTheGraceWindowWithNoCheckIn_Expires()
    {
        var appointment = NewScheduledAppointment();

        appointment.ExpireIfOverdue(WindowEnd.AddHours(1));

        Assert.Equal(AppointmentStatus.Expired, appointment.Status);
    }

    [Fact]
    public void ExpireIfOverdue_AfterCheckIn_DoesNothing()
    {
        var appointment = NewScheduledAppointment();
        appointment.CheckIn(WindowStart, "lane-1");

        appointment.ExpireIfOverdue(WindowEnd.AddHours(1));

        Assert.Equal(AppointmentStatus.CheckedIn, appointment.Status);
    }
}
