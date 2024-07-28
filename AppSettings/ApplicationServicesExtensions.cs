using IdentityWebApiSample.Server.Infrastructures;
using IdentityWebApiSample.Server.Services;

namespace IdentityWebApiSample.Server.AppSettings;

public static class ApplicationServicesExtensions
{
    public static void AddApplicationServicesExtension(this IServiceCollection services)
    {
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }
}
