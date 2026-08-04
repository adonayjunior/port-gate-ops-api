using GateOps.Application.GateOperations;
using GateOps.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GateOps.Infrastructure;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Wires up persistence with EF Core's InMemory provider — enough to run and
    /// demo the API end-to-end without external infrastructure. Swapping to
    /// SQL Server/PostgreSQL for a real deployment is a one-line change here
    /// (UseInMemoryDatabase -> UseSqlServer/UseNpgsql), the rest of the app is
    /// provider-agnostic by design (Clean Architecture: Infrastructure is the
    /// only layer that knows this detail exists).
    /// </summary>
    public static IServiceCollection AddGateOpsInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<GateOpsDbContext>(options => options.UseInMemoryDatabase("GateOps"));
        services.AddScoped<IGateAppointmentRepository, GateAppointmentRepository>();
        return services;
    }
}
