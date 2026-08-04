using GateOps.Domain.GateOperations;
using Microsoft.EntityFrameworkCore;

namespace GateOps.Infrastructure.Persistence;

public sealed class GateOpsDbContext(DbContextOptions<GateOpsDbContext> options) : DbContext(options)
{
    public DbSet<GateAppointment> GateAppointments => Set<GateAppointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GateAppointment>(builder =>
        {
            builder.HasKey(a => a.Id);

            // ContainerNumber and VehiclePlate are value objects wrapping a single
            // string — mapped via a value converter so each still occupies just one
            // column/field, with EF Core doing the round-trip through Create().
            builder.Property(a => a.ContainerNumber)
                .HasConversion(cn => cn.Value, raw => ContainerNumber.Create(raw))
                .HasMaxLength(11);

            builder.Property(a => a.VehiclePlate)
                .HasConversion(vp => vp.Value, raw => VehiclePlate.Create(raw))
                .HasMaxLength(10);

            builder.Property(a => a.Status).HasConversion<string>();
            builder.Property(a => a.Direction).HasConversion<string>();
        });
    }
}
