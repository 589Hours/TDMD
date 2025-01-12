using Routey.Domain.Models;
using Routey.Domain.SQLiteDatabases.Entities;

namespace Routey.Domain.SQLiteDatabases
{
    public interface IRouteDatabase
    {
        Task<IEnumerable<RouteEntity>> GetRoutesAsync();
        Task AddRouteAsync(Route route);
        Task DeleteRouteAsync(RouteEntity routeEntity);
    }
}
