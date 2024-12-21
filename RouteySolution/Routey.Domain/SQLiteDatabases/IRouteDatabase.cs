using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Routey.Domain.Models;
using Routey.Domain.SQLiteDatabases.Entities;

namespace Routey.Domain.SQLiteDatabases
{
    public interface IRouteDatabase
    {
        Task<IEnumerable<RouteEntity>> GetRoutesAsync();
        Task AddRouteAsync(RouteEntity routeEntity);
    }
}
