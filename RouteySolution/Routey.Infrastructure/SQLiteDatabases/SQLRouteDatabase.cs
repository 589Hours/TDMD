using System.Globalization;
using Routey.Domain.Models;
using Routey.Domain.SQLiteDatabases;
using Routey.Domain.SQLiteDatabases.Entities;
using SQLite;

namespace Routey.Infrastructure.SQLiteDatabases
{
    /// <summary>
    /// This class has been documented with the help of GitHub Copilot!
    /// SQLite database object that can be used to perform CRUD operations on the RouteEntity table.
    /// </summary>
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

        /// <summary>
        /// Initializes the database connection and creates the RouteEntity table if it doesn't exist.
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            if (this.db != null)
                return;

            this.db = new SQLiteAsyncConnection(DbPath, Flags);
            var result = await db.CreateTableAsync<RouteEntity>();
            await CreateTestData();
        }

        public async Task AddRouteAsync(Route route)
        {
            await Init();

            await db.InsertAsync(route.ConvertToRouteEntity());
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

        /// <summary>
        /// Helper method that creates test data in the database.
        /// </summary>
        /// <returns></returns>
        public async Task CreateTestData()
        { 
            Route route1 = new Route("route 1", DateTime.ParseExact("24-12-2024 15:30", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture));
            route1.TotalDistance = 1;
            route1.AmountOfRoutePoints = 4;
            route1.SumOfSpeeds = 12;
            route1.TotalDuration = "01:22:00";

            Route route2 = new Route("route 2", DateTime.ParseExact("23-12-2024 17:30", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture));
            route2.TotalDistance = 5;
            route2.AmountOfRoutePoints = 10;
            route2.SumOfSpeeds = 50;
            route2.TotalDuration = "12:22:33";

            await AddRouteAsync(route1);
            await AddRouteAsync(route2);
        }
    }
}