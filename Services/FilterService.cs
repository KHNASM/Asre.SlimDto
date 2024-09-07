using Asre.SlimDto.Abstract;
using System.Collections.Concurrent;
using System.Collections;
using System.Dynamic;
using System.Reflection;
using Asre.SlimDto.Attributes;

namespace Asre.SlimDto.Services
{
    public class FilterService : IFilterService
    {
        // A thread-safe cache for storing property metadata for each entity type
        private readonly ConcurrentDictionary<Type, List<PropertyInfo>> _propertyCache = new();

        /// <summary>
        /// Asynchronously filters properties of an object (or collection of objects) that are marked with the DtoProperty attribute.
        /// Uses cached metadata for better performance.
        /// </summary>
        /// <param name="entity">The object or collection to filter.</param>
        /// <returns>A task that resolves to a dynamic object or a collection of dynamic objects with only the DtoProperty marked properties.</returns>
        public async Task<dynamic> FilterPropertiesAsync(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Check if the input is a collection (array, list, etc.)
            if (entity is IEnumerable entityCollection && entity is not string)
            {
                // Process the collection asynchronously
                var resultList = new List<dynamic>();
                foreach (var item in entityCollection)
                {
                    var filteredItem = await FilterSingleDtoPropertiesAsync(item);
                    resultList.Add(filteredItem);
                }
                return resultList;
            }

            // If it's a single object, apply the filtering logic directly
            return await FilterSingleDtoPropertiesAsync(entity);
        }

        /// <summary>
        /// Asynchronously filters properties for a single object marked with the DtoProperty attribute.
        /// </summary>
        /// <param name="entity">The object to filter.</param>
        /// <returns>A task that resolves to a dynamic object with only the DtoProperty marked properties.</returns>
        private async Task<dynamic> FilterSingleDtoPropertiesAsync(object entity)
        {
            Type entityType = entity.GetType();

            // Get or add the cached properties for this entity type
            var dtoProperties = _propertyCache.GetOrAdd(entityType, GetDtoProperties);

            // Process the properties asynchronously if needed
            return await Task.Run(() =>
            {
                // Build the dynamic object with only the filtered properties
                IDictionary<string, object> dtoObject = new ExpandoObject();

                foreach (var property in dtoProperties)
                {
                    dtoObject[property.Name] = property.GetValue(entity);
                }

                return (dynamic)dtoObject;
            });
        }

        /// <summary>
        /// Retrieves all properties marked with DtoProperty attribute for a given type using reflection.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <returns>A list of properties decorated with DtoProperty attribute.</returns>
        private List<PropertyInfo> GetDtoProperties(Type entityType)
        {
            // Use reflection to get properties marked with DtoProperty attribute
            return entityType.GetProperties()
                             .Where(prop => prop.GetCustomAttribute<SlimCandidate>() != null)
                             .ToList();
        }
    }
}
