# Asre.SlimDto

**Asre.SlimDto** is a flexible and performant C# library designed for dynamic filtering of Data Transfer Objects (DTOs) based on custom attributes. It leverages caching and asynchronous operations to optimize performance, especially for large objects and collections. The library allows you to define which properties of your entities should be exposed using the `[SlimCandidate]` attribute, eliminating the need for manual creation of DTO classes.

## Features
- **Dynamic DTO Filtering**: Filter out properties of an entity marked with the `[SlimCandidate]` attribute.
- **Supports Collections**: Automatically handles lists, arrays, and other enumerable collections of entities.
- **Caching**: Caches property metadata (reflection results) to improve performance on subsequent calls.
- **Asynchronous**: Methods are fully asynchronous, making the library suitable for high-performance and scalable applications.
- **Thread-Safe**: Designed to handle concurrent requests in multi-threaded environments like ASP.NET Core.
- **Singleton in DI**: The `FilterService` should be registered as a singleton in the Dependency Injection container.


