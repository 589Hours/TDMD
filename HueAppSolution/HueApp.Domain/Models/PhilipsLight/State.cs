using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HueApp.Domain.Models.PhilipsLight
{
    public class State
    {
        public bool on { get; set; }
        public int bri { get; set; }
        public int hue { get; set; }
        public int sat { get; set; }

        public override string ToString()
        {
            return $"On: {on}\n" +
                   $"Brightness: {bri}\n" +
                   $"Hue: {hue}\n" +
                   $"Saturation: {sat}\n";
        }
    }
}
