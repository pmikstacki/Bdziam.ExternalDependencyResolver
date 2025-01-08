# Bdziam.ExternalDependencyResolver

Bdziam.ExternalDependencyResolver is a C# library that provides a mechanism to resolve dependencies for a given type using a combination of a service provider and additional dependencies.

## Features

- Resolves dependencies using a service provider.
- Supports additional dependencies.
- Allows overriding dependencies with additional ones.

## Installation

To install Bdziam.ExternalDependencyResolver, add the following package to your project:

```bash
dotnet add package Bdziam.ExternalDependencyResolver
```

## Usage

### Registering the Resolver

To register the `ExternalDependencyResolver` in your `IServiceCollection`, use the `AddExternalDependencyResolver` extension method:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Bdziam.ExternalDependencyResolver;

var services = new ServiceCollection();
services.AddExternalDependencyResolver();
```

You can also provide a custom `IServiceProvider`:

```csharp
var customServiceProvider = new ServiceCollection().BuildServiceProvider();
services.AddExternalDependencyResolver(customServiceProvider);
```

### Resolving Dependencies

To resolve dependencies, use the `ExternalDependencyResolver` class:

```csharp
using Bdziam.ExternalDependencyResolver;
using System;
using System.Collections.Generic;

var serviceProvider = new ServiceCollection().BuildServiceProvider();
var resolver = new ExternalDependencyResolver(serviceProvider);

var instance = resolver.Resolve<MyClass>();
```

You can also provide additional dependencies and override existing ones:

```csharp
var additionalDependencies = new KeyValuePair<Type, object>[]
{
    new KeyValuePair<Type, object>(typeof(IMyDependency), new MyDependency())
};

var instance = resolver.Resolve<MyClass>(true, additionalDependencies);
```

## Testing

The project includes unit tests using xUnit and Moq. To run the tests, use the following command:

```bash
dotnet test
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MPL License. See the `LICENSE` file for details.