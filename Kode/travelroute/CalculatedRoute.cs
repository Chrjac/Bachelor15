using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelroute
{
    public class CalculatedRoute
    {
        public string Start { get; set; }
        public string Stopp { get; set; }
        public List<String> Via { get; set; }
        public int Vehicle { get; set; }
        public int Distance { get; set; }
        public String Time { get; set; }
        public int TotalCostSmall { get; set; }
        public int TotalCostLarge { get; set; }
        public int NumbersOfBarriers { get; set; }
        public int FerryCount { get; set; }
        public List<Barrier> Barriers { get; set; }
        public Directions Directions { get; set; }
        public List<Tuple<double, double>> Coordinates { get; set; }
        public ToHRessurs ResultToImport { get; set; }


    }
}
