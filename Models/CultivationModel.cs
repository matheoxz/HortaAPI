using System;

namespace HortaIoT.Models
{
    public class CultivationModel
    {
        public int Id {get;set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public long Start {get; set;}
        public long Finish {get; set;}        
    }
}