using System;

namespace HortaIoT.Models
{
    public class DataModel
    {
        public long Received {get;set;}
        public double Ph {get; set;}
        public double Ec {get; set;}
        public double Temperature {get; set;}
        public double Turbidity {get; set;}
        public int Illuminance {get; set;}
        public int WaterLevel {get; set;}
        
    }
}