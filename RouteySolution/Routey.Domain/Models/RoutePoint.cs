using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routey.Domain.Models
{
    public class RoutePoint
    {
        public double longtitude { get; set; }
        public double latitude { get; set; }
        public double speed { get; set; }

        public RoutePoint(double longtitude, double latitude, double speed)
        {
            this.longtitude = longtitude;
            this.latitude = latitude;
            this.speed = speed;
        }
    }
}
