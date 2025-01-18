using SQLite;

namespace Routey.Domain.SQLiteDatabases.Entities
{
    /// <summary>
    /// This class has been documented with the help of GitHub Copilot!
    /// RouteEntity class is a model class for the RouteEntity table in the SQLite database.
    /// </summary>
    public class RouteEntity
    {
        [PrimaryKey]
        public DateTime RouteDateTime { get; set; }
        public string RouteName { get; set; }
        public double TotalDistance { get; set; }
        public double AverageSpeed { get; set; }
        public string RouteDuration { get; set; }
    }
}