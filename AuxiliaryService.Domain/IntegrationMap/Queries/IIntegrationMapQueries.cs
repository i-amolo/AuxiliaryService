using AuxiliaryService.Domain.IntegrationMap.Repositories;
using NPoco;

namespace AuxiliaryService.Domain.IntegrationMap.Queries
{
    public interface IIntegrationMapQueries
    {
        /// <summary>
        /// SQL query to retrieve maps by criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Sql Select_GetMapByCriteria(IntegrationMapCriteria criteria);
    }
}