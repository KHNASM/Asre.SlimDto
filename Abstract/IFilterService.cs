using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asre.SlimDto.Abstract
{
    public interface IFilterService
    {
        /// <summary>
        /// Asynchronously filters properties of an object or collection that are marked with the DtoProperty attribute.
        /// </summary>
        /// <param name="entity">The object or collection to filter.</param>
        /// <returns>A task that resolves to a dynamic object or a collection of dynamic objects with only the DtoProperty marked properties.</returns>
        Task<dynamic> FilterPropertiesAsync(object entity);
    }
}
