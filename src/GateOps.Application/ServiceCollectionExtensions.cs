using GateOps.Application.GateOperations;
using Microsoft.Extensions.DependencyInjection;

namespace GateOps.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGateOpsApplication(this IServiceCollection services)
    {
        services.AddScoped<IGateAppointmentService, GateAppointmentService>();
        return services;
    }
}
