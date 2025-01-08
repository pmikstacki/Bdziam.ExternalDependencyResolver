namespace Bdziam.ExternalDependencyResolver;
/// <summary>
/// The `ExternalDependencyResolver` class provides a mechanism to resolve dependencies for a given type using a combination of a service provider and additional dependencies.
/// </summary>
public class ExternalDependencyResolver(IServiceProvider serviceProvider)
{
    /// <summary>
    /// Resolves an instance of type `T` by:
    /// 1. Identifying the constructor of `T` with the most parameters.
    /// 2. Resolving each parameter using:
    ///    - Additional dependencies if `overrideDependencies` is true.
    ///    - The service provider.
    ///    - Additional dependencies if not found in the service provider.
    /// 3. Throwing an exception if any parameter cannot be resolved.
    /// 4. Creating and returning an instance of `T` using the resolved parameters.
    /// <param name="overrideDependencies">if true, it prioritizes recognized types from additionalDependencies when resolving constructor parameters</param>
    /// <param name="additionalDependencies">additional dependencies to use for resolving constructor parameters</param>
    /// <returns>Resolved Type from container</returns>
    /// </summary>
    public T Resolve<T>(bool overrideDependencies = false, params KeyValuePair<Type, object>[] additionalDependencies)
    {
        // Get the type's constructor with the most parameters (usually the main constructor)
        var constructor = typeof(T).GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length)
            .First();

        // Get all parameters that need to be resolved
        var parameters = constructor.GetParameters();
        
        // Resolve all required parameters
        var resolvedParameters = new object[parameters.Length];
        var additionalDependencyMap = additionalDependencies.ToDictionary();
        for (int i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var parameterType = parameter.ParameterType;

            // Check if we have this type in additional dependencies and if we should use it
            if (overrideDependencies &&
                additionalDependencyMap.TryGetValue(parameterType, out var additionalDependency))
            {
                resolvedParameters[i] = additionalDependency;
                continue;
            }

            // Try to resolve from service provider first
            var service = serviceProvider.GetService(parameterType);
            if (service != null)
            {
                resolvedParameters[i] = service;
                continue;
            }

            // If not found in service provider and we have it in additional dependencies, use it
            if (additionalDependencyMap.TryGetValue(parameterType, out additionalDependency))
            {
                resolvedParameters[i] = additionalDependency;
                continue;
            }

            // If we can't resolve the dependency, throw an exception
            throw new InvalidOperationException(
                $"Could not resolve parameter '{parameter.Name}' of type {parameterType.Name} for type {typeof(T).Name}");
        }

        // Create instance with resolved parameters
        return (T)constructor.Invoke(resolvedParameters);
    }
}