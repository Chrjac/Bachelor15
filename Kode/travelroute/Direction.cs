using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelroute
{
    public class Direction
    {
     
        public Direction(string roadNumber, string textDescription)
        {
   
            this.RoadNumber = roadNumber;
            this.TextDescription = textDescription;
        }
        public String RoadNumber { get; set; }
        public String TextDescription { get; set; }
    }
}
