using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelroute
{
   public interface ITravelRoute
    {
         CalculatedRoute Calculate(travelroute.Route input);
        
    }
}
