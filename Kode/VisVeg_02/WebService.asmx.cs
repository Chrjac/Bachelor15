using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace VisVeg_02
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]

    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public travelroute.CalculatedRoute PassToTravelRoute(string[] Start, string[] Stopp, string[][] Via, int Vehicle, string Comment)
        {
            var viaList = new List<travelroute.Point>();

            Debug.WriteLine(Comment);
            if (Via != null)
            {
                for (int i = 0; i < Via.Count(); i++)
                {
                    var list = new travelroute.Point(Via[i][0], Via[i][1], Via[i][2]);
                    /* list.Add(Via[i][0]);
                     list.Add(Via[i][1]);
                     list.Add(Via[i][2]);*/
                    viaList.Add(list);
                }
            }

            travelroute.Route selectedRoute = new travelroute.Route
            {
                Start = new travelroute.Point(Start[0], Start[1], Start[2]),
                Stopp = new travelroute.Point(Stopp[0], Stopp[1], Stopp[2]),
                Via = new List<travelroute.Point>(viaList),
                Vehicle = Vehicle,
                Comment = Comment

            };



            travelroute.ITravelRoute R = new Ruteplantjenesten.TravelRoute();
            var result = R.Calculate(selectedRoute);



            return result;
        }


    }

}
