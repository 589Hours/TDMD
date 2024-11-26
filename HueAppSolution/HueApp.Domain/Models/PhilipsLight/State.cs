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
        public float[] xy { get; set; }
        public int ct { get; set; }
        public string alert { get; set; }
        public string effect { get; set; }
        public string colormode { get; set; }
        public bool reachable { get; set; }

        public override string ToString()
        {
            return $"On: {on}\n" +
                   $"Brightness: {bri}\n" +
                   $"Hue: {hue}\n" +
                   $"Saturation: {sat}\n" +
                   $"XY Coordinates: {xy[0]} {xy[1]}\n" +
                   $"Color Temperature: {ct}\n" +
                   $"Alert: {alert}\n" +
                   $"Effect: {effect}\n" +
                   $"Color Mode: {colormode}\n" +
                   $"Reachable: {reachable}";
        }
    }
}
