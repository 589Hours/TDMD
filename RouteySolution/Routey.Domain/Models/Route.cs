using Routey.Domain.SQLiteDatabases.Entities;

namespace Routey.Domain.Models
{
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

        public double GetAverageSpeed()
        {
            if (SumOfSpeeds == null || AmountOfRoutePoints == 0)
                return 0;

            return (double) SumOfSpeeds / AmountOfRoutePoints;
        }

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
