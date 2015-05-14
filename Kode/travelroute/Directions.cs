using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelroute
{
    public class Directions
    {
        
        public Directions(List<RoadDescription> fullRoadDescription, String compressedRoadDescription)
        {
            
            this.FullRoadDescription = fullRoadDescription;
            this.CompressedRoadDescription = compressedRoadDescription;
        }
        public List<travelroute.RoadDescription> FullRoadDescription { get; set; }
        public String CompressedRoadDescription { get; set; }
    }
}
