using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routey.Domain.Models
{
    public class Route
    {
        // TODO: Check later if datetimes are necessary (totaldistance / avg. speed)
        public string name { get; set; }
        public List<RoutePoint> routePoints { get; set; }
        public double totalDistance { get; set; }
        public DateTime startRouteMoment { get; set; }
        public DateTime endRouteMoment { get; set; }

        public Route() { routePoints = new List<RoutePoint>(); }
        public Route(string name, List<RoutePoint> points)
        {
            this.name = name;
            this.routePoints = points;
        }

        public double GetTotalRouteDistance()
        {
            //TODO: add business logic for calculating the total distance
            throw new NotImplementedException();
        }

        public TimeSpan GetTotalRouteDuration()
        {
            // end - start = tijdsverschil
            throw new NotImplementedException();
        }

        public double GetAverageSpeed()
        {
            throw new NotImplementedException();
        }

    }
}
