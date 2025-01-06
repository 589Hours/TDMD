using System.Globalization;
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

        private string DbPath;

        private SQLiteAsyncConnection db;

        public SQLRouteDatabase(string path)
        {
            this.DbPath = path;
            Task.Run(async () =>
            {
                await Init();
            });
        }

        public async Task Init()
        {
            if (this.db != null)
                return;

            if (!Path.Exists(DbPath))
                File.Create(DbPath).Close();

            this.db = new SQLiteAsyncConnection(DbPath, Flags);
            var result = await db.CreateTableAsync<RouteEntity>();
            await CreateTestData();
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

        public async Task<IEnumerable<RouteEntity>> GetRoutesAsync()
        {
            await Init();

            return await db.Table<RouteEntity>().ToListAsync();
        }

        public async Task DeleteRouteAsync(RouteEntity routeEntity)
        {
            await Init();

            await db.DeleteAsync(routeEntity);
        }

        public async Task CreateTestData() // TODO fix bug writing test data into db3-file
        {
            Route route1 = new Route();
            route1.name = "route 1";
            route1.startRouteMoment = DateTime.ParseExact("24-12-2024 15:30", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            route1.endRouteMoment = DateTime.ParseExact("24-12-2024 16:00", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            route1.totalDistance = 1;
            route1.routePoints = new List<RoutePoint>() { new RoutePoint(10, 10, 5) };

            Route route2 = new Route();
            route2.name = "route 2";
            route2.startRouteMoment = DateTime.ParseExact("23-12-2024 17:30", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            route2.endRouteMoment = DateTime.ParseExact("23-12-2024 18:00", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            route2.totalDistance = 5;
            route2.routePoints = new List<RoutePoint>() { new RoutePoint(30, 10, 10) };

            await AddRouteAsync(route1);
            await AddRouteAsync(route2);
        }

        //TODO: Database wipe method/ check list usage

    }
}
