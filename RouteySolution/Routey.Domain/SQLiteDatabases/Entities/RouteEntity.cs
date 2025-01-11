using SQLite;

namespace Routey.Domain.SQLiteDatabases.Entities
{
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
