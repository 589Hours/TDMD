using Routey.Domain.Models;
using Routey.Domain.SQLiteDatabases.Entities;

namespace Routey.Domain.SQLiteDatabases
{
    /// <summary>
    /// This class has been documented with the help of GitHub Copilot!
    /// Database actions that can be performed on the RouteEntity table.
    /// </summary>
    public interface IRouteDatabase
    {
        Task<IEnumerable<RouteEntity>> GetRoutesAsync();
        Task AddRouteAsync(Route route);
        Task DeleteRouteAsync(RouteEntity routeEntity);
    }
}