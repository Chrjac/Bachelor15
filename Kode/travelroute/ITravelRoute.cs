using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelroute
{
   public interface ITravelRoute
    {
         Result Search(travelroute.Route input);
         Result CalculateResultFromJson(String jsonInput);
         Directions CalculateRoadDescriptionDirectionsObject(travelroute.Result result);
         CalculatedRoute Calculate(travelroute.Route input);
         ToHRessurs CalculateHRessursObject(travelroute.Route input, travelroute.Result result,Directions listOfRoadDescription);
         CalculatedRoute CalcReturn(travelroute.Route input, travelroute.Result result, Directions listOfRoadDescription, travelroute.ToHRessurs finalResult);
    }
}
