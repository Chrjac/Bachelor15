using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelroute
{
    public class RoadDescription
    {


        public RoadDescription(string roadNumber, List<RoadDescriptionText> textDescription)
        {

            this.RoadNumber = roadNumber;
            this.TextDescription = textDescription;
        }
        public String RoadNumber { get; set; }
        public List<RoadDescriptionText> TextDescription { get; set; }
    }
}
