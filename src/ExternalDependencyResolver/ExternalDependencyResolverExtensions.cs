using Microsoft.Extensions.DependencyInjection;

namespace Bdziam.ExternalDependencyResolver;

public static class ExternalDependencyResolverExtensions
{
    public static IServiceCollection AddExternalDependencyResolver(this IServiceCollection services, IServiceProvider? customServiceProvider = null)
    {
        return customServiceProvider != null ? services.AddSingleton<ExternalDependencyResolver>(sp => new ExternalDependencyResolver(customServiceProvider)) : services.AddSingleton<ExternalDependencyResolver>();
    }
}