using System;
using System.Collections.Generic;

namespace AuxiliaryService.Domain.IntegrationMap.Repositories
{
    public interface IIntegrationMapRepository
    {
        /// <summary>
        /// Save maps.
        /// </summary>
        /// <param name="dtos"></param>
        void Create(IEnumerable<IntegrationMapDto> dtos);

        /// <summary>
        /// Get maps by criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>List of integration map.</returns>
        List<IntegrationMapDto> GetByCriteria(IntegrationMapCriteria criteria);
    }
}