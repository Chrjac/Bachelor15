using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace travelroute
{
    public class Route
    {
        public Point Start { get; set; }
        public Point Stopp { get; set; }
        public List<Point> Via { get; set; }
        public int Vehicle { get; set; }
        public String Comment { get; set; }

    }
}