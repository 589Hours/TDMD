using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApp.Domain.Models.PhilipsLight
{
    public class Light
    {
        public State state { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string modelid { get; set; }
        public string swversion { get; set; }
        public string uniqueid { get; set; }
        public Pointsymbol pointsymbol { get; set; }

        public override string ToString()
        {
            return 
                $"Name: {name}\n" +
                $"State: {state}";
        }
    }
}
