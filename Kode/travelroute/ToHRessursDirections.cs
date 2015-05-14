using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelroute
{
    public class ToHRessursDirections
    {
        public string Start { get; set; }
        public string Stopp { get; set; }
        public List<String> Via { get; set; }
        public int Vehicle { get; set; }
        public int Distance { get; set; }
        public String CompressedDirections { get; set; }
        public String Comment { get; set; }
    }
}
