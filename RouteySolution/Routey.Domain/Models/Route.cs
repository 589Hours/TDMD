using Routey.Domain.SQLiteDatabases.Entities;

namespace Routey.Domain.Models
{
    /// <summary>
    /// This class has been documented with the help of GitHub Copilot!
    /// This class represents a route that the user is walking/ has walked.
    /// </summary>
    public class Route
    {
        public string name { get; set; }
        public double TotalDistance { get; set; }
        public string TotalDuration { get; set; }
        public DateTime startRouteMoment { get; set; }
        public double? SumOfSpeeds { get; set; }
        public int AmountOfRoutePoints { get; set; }

        public Route(string name, DateTime startRouteMoment) 
        {
            this.name = name;
            this.startRouteMoment = startRouteMoment;
            this.TotalDistance = 0;
            this.TotalDuration = "00:00:00";
            this.SumOfSpeeds = 0;
            this.AmountOfRoutePoints = 0;
        }

        /// <summary>
        /// Calculates the average speed of the route.
        /// </summary>
        /// <returns></returns>
        public double GetAverageSpeed()
        {
            if (SumOfSpeeds == null || AmountOfRoutePoints == 0)
                return 0;

            return (double) SumOfSpeeds / AmountOfRoutePoints;
        }

        /// <summary>
        /// Converts a Route object to a RouteEntity object. This RouteEntity can then be stored in the database.
        /// </summary>
        /// <returns></returns>
        public RouteEntity ConvertToRouteEntity()
        {
            return new RouteEntity
            {
                RouteDateTime = this.startRouteMoment,
                RouteName = this.name,
                AverageSpeed = GetAverageSpeed(),
                TotalDistance = Double.Round(this.TotalDistance, 2),
                RouteDuration = this.TotalDuration
            };
        }
    }
}