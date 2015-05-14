using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace travelroute
{
    public class Result
    {
        public int Distance { get; set; }
        public int PriceSmall { get; set; }
        public int PriceTruck { get; set; }
        public String Time { get; set; }
        public int NumbersOfBarriers { get; set; }
        public int FerryCount { get; set; }
        public List<List<String>> Directions { get; set; }
        public List<Barrier> Barriers { get; set; }
        public List<Tuple<double, double>> Coordinates { get; set; }

    }
}
