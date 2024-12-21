using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Routey.Domain.Models;
using Routey.Domain.SQLiteDatabases;
using Routey.Domain.SQLiteDatabases.Entities;
using SQLite;

namespace Routey.Infrastructure.SQLiteDatabases
{
    public class SQLRouteDatabase : IRouteDatabase
    {
        public const SQLite.SQLiteOpenFlags Flags =
             // Open the database in read/write mode
             SQLite.SQLiteOpenFlags.ReadWrite |
             // Create the database if it doesn't exist
             SQLite.SQLiteOpenFlags.Create |
             // Enable multi-threaded database access
             SQLite.SQLiteOpenFlags.SharedCache;

        private readonly SQLiteAsyncConnection db;

        public SQLRouteDatabase(string dbPath)
        {
            this.db = new SQLiteAsyncConnection(dbPath, Flags);
        }

        public async Task Init()
        {
            var result = await db.CreateTableAsync<RouteEntity>();
        }

        public async Task AddRouteAsync(Route route)
        {
            await Init();

            await db.InsertAsync(new RouteEntity { 
                RouteDateTime = route.startRouteMoment,
                RouteName = route.name,
                AverageSpeed = route.GetAverageSpeed(),
                TotalDistance = route.GetTotalRouteDistance(),
                RouteDuration = route.GetTotalRouteDuration()
            });
        }

        public Task<IEnumerable<RouteEntity>> GetRoutesAsync()
        {
            throw new NotImplementedException();
        }

        public Task AddRouteAsync(RouteEntity routeEntity)
        {
            throw new NotImplementedException();
        }
    }
}
