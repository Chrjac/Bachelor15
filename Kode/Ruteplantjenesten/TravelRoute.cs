using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Ruteplantjenesten
{
    public class TravelRoute : travelroute.ITravelRoute
    {
        public travelroute.CalculatedRoute Calculate(travelroute.Route input)
        {          
            var barrierAndDistanceObject = Search(input); 
            var calculatedDirectionList = CalculateRoadDescriptionDirectionsObject(barrierAndDistanceObject);
            var calculatedHRessursObject = CalculateHRessursObject(input, barrierAndDistanceObject, calculatedDirectionList);
            var calculatedResult = CalcReturn(input, barrierAndDistanceObject, calculatedDirectionList, calculatedHRessursObject);

            return calculatedResult;
        }


        public travelroute.Result Search(travelroute.Route input)
        {
            string start = input.Start.Xcoordinate + "," + input.Start.Ycoordinate;
            Debug.WriteLine(start);
            string stop = input.Stopp.Xcoordinate + "," + input.Stopp.Ycoordinate;
            string via = "";

            foreach(var i in input.Via)
            {
                via = via + i.Xcoordinate + "," + i.Ycoordinate + ";";
            }

            string url = "https://www.vegvesen.no/ruteplan/routingservice_v1_0/routingservice/solve?&stops=" + start + ";" + via + stop + "&barriers&format=json&lang=nb-NO";

            NetworkCredential myCredentials = new NetworkCredential("", "");
            myCredentials.UserName = "TjeRuteplanChja";
            myCredentials.Password = "7LJ7jZETcN";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Credentials = myCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader stream = new StreamReader(response.GetResponseStream());

                string final = stream.ReadToEnd();

                var test = response.StatusCode;
               
                    final.Contains("directions");
                    var b = CalculateResultFromJson(final);
                    return b;
                
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


        }
        public travelroute.Result CalculateResultFromJson(string jsonInput)
        {
            //try?
            JObject o = JObject.Parse(jsonInput);

            var distance = int.Parse(o["directions"][0]["summary"]["totalLength"].ToString());
            var numbersOfBarriers = int.Parse(o["directions"][0]["summary"]["statistics"][3]["value"].ToString());
            int totalCostSmall = int.Parse(o["directions"][0]["summary"]["statistics"][0]["value"].ToString());
            int totalCostLarge = int.Parse(o["directions"][0]["summary"]["statistics"][1]["value"].ToString());
            int ferryCount = int.Parse(o["directions"][0]["summary"]["statistics"][2]["value"].ToString());
            var barrierList = new List<travelroute.Barrier>();
            var time = o["directions"][0]["summary"]["totalDriveTime"].ToString();

            foreach (var i in o["directions"][0]["features"])
            {
                if (i["attributes"]["roadFeatures"].HasValues == true)
                {
                    if (i["attributes"]["roadFeatures"][0].HasValues == true)
                    {
       
                        foreach (var k in i["attributes"]["roadFeatures"])
                        {
                            if (k["attributeType"].ToString().Equals("nvdb:Bomstasjon"))
                            {
                                var list = new List<String>();
                                var priceSmall = k["values"][0]["value"].ToString();
                                var priceLarge = k["values"][1]["value"].ToString();
                                var barrierObjekt = new travelroute.Barrier(k["values"][2]["value"].ToString(), int.Parse(priceSmall), int.Parse(priceLarge));
                                barrierList.Add(barrierObjekt);
                            }
                        }
                    }
                }
            }
            var compressedGeometryList = new List<String>();


            foreach (var i in o["directions"][0]["features"])
            {
                var b =i["compressedGeometry"].ToString();
                compressedGeometryList.Add(b);

            }

            CoordinateManaging.Decompresser R = new CoordinateManaging.Decompresser();
            var result = R.Start(compressedGeometryList);

            CoordinateManaging.ConverterUtmToLatLng B = new CoordinateManaging.ConverterUtmToLatLng();
            var converted = B.ConvertAll(result);

            var directionList = new List<List<String>>();

            foreach (var a in o["directions"][0]["features"])
            {

                var list = new List<String>();
                var dir = a["attributes"]["text"];
                list.Add(dir.ToString());

                directionList.Add(list);
            }

            travelroute.Result barrierAndDistanceObject = new travelroute.Result
            {
                Distance = distance,
                Barriers = new List<travelroute.Barrier>(barrierList),
                NumbersOfBarriers = numbersOfBarriers,
                PriceSmall = totalCostSmall,
                Time = time,
                PriceTruck = totalCostLarge,
                Directions = directionList,
                Coordinates = converted,
                FerryCount = ferryCount

            };

            return barrierAndDistanceObject;
        }


        public travelroute.Directions CalculateRoadDescriptionDirectionsObject(travelroute.Result result)
        {
            var roadNumberAndDescriptionSeparated = new List<travelroute.Direction>();

            foreach (var k in result.Directions)
            {
                String str = k[0].ToString();
                String[] res = str.Split('}');
                res[0] = res[0].Substring(1);

                var onearray = new travelroute.Direction(res[0], res[1]);
                roadNumberAndDescriptionSeparated.Add(onearray);
            }

            var lastRoadNumber = "";
            var groupedDirectionsByRoadNumber = new List<travelroute.RoadDescription>();
            for (var o = 0; o < roadNumberAndDescriptionSeparated.Count(); )
            {
                if (roadNumberAndDescriptionSeparated[o].RoadNumber != lastRoadNumber)
                {
                    lastRoadNumber = roadNumberAndDescriptionSeparated[o].RoadNumber;

                    List<travelroute.RoadDescriptionText> oneRoadDir = new List<travelroute.RoadDescriptionText>();
                    var j = 0;
                    for (j = o; (j < roadNumberAndDescriptionSeparated.Count()) && (roadNumberAndDescriptionSeparated[j].RoadNumber == lastRoadNumber); j++)
                    {
                        String singleRoadDescription = roadNumberAndDescriptionSeparated[j].TextDescription;

                        oneRoadDir.Add(new travelroute.RoadDescriptionText(singleRoadDescription));
                    }
                    groupedDirectionsByRoadNumber.Add(new travelroute.RoadDescription(lastRoadNumber, oneRoadDir));

                    o = j;
                }
                else
                {
                    o++;
                }
            }

            var compressedDirectionList = "Kjørerute via ";
    
            for (int a = 0; a < groupedDirectionsByRoadNumber.Count(); a++)
            {
                var startIndex = groupedDirectionsByRoadNumber[a].RoadNumber.ToString().Substring(0, 1);
                if (startIndex.Equals("K") || startIndex.Equals("P"))
                {
                    Debug.WriteLine(groupedDirectionsByRoadNumber[a].RoadNumber);
                }
                else
                {
                    if (a < groupedDirectionsByRoadNumber.Count() - 2)
                        compressedDirectionList = compressedDirectionList + groupedDirectionsByRoadNumber[a].RoadNumber + ", ";
                    else if (a < groupedDirectionsByRoadNumber.Count() - 1)
                        compressedDirectionList = compressedDirectionList + groupedDirectionsByRoadNumber[a].RoadNumber + " og ";
                    else
                        compressedDirectionList = compressedDirectionList + groupedDirectionsByRoadNumber[a].RoadNumber;
                }
            }
 
            var allDirections = new travelroute.Directions(groupedDirectionsByRoadNumber, compressedDirectionList);
           
            return allDirections;
        }

        public travelroute.ToHRessurs CalculateHRessursObject(travelroute.Route input, travelroute.Result result, travelroute.Directions listOfRoadDescription)
        {
            var viaList = new List<String>();

            for (int i = 0; i < input.Via.Count(); i++)
            {
                var list = input.Via[i].Name;
                viaList.Add(list);
            }

            var totalCosts = 0;
            if (input.Vehicle == 1)
            {
                totalCosts = 0;
            }
            else if (input.Vehicle == 2)
            {
                totalCosts = result.PriceSmall;
            }
            else if (input.Vehicle == 3)
            {
                totalCosts = result.PriceTruck;
            }

            travelroute.ToHRessursDirections finalDirectionResult = new travelroute.ToHRessursDirections
            {
                Start = input.Start.Name,
                Stopp = input.Stopp.Name,
                Via = new List<String>(viaList),
                Vehicle = input.Vehicle,
                Distance = result.Distance,
                CompressedDirections = listOfRoadDescription.CompressedRoadDescription,
                Comment = input.Comment
            };
            travelroute.ToHRessursBarriers finalBarriersResult = new travelroute.ToHRessursBarriers
            {
                Barriers = result.Barriers,
                TotalCost = totalCosts,
            };

            travelroute.ToHRessurs finalResult = new travelroute.ToHRessurs
            {
                BarrierInfo = finalBarriersResult,
                DirectionInfo = finalDirectionResult
            };

            return finalResult;

        }


        public travelroute.CalculatedRoute CalcReturn(travelroute.Route input, travelroute.Result result, travelroute.Directions listOfRoadDescription, travelroute.ToHRessurs finalResult)
        {

            travelroute.CalculatedRoute routeInfo = new travelroute.CalculatedRoute
            {
                Start = input.Start.Name,
                Stopp = input.Stopp.Name,
                Via = finalResult.DirectionInfo.Via,
                Vehicle = input.Vehicle,
                Distance = result.Distance,
                NumbersOfBarriers = result.NumbersOfBarriers,
                Barriers = result.Barriers,
                FerryCount = result.FerryCount,
                Time = result.Time,
                Directions = listOfRoadDescription,
                TotalCostSmall = result.PriceSmall,
                TotalCostLarge = result.PriceTruck,
                Coordinates = result.Coordinates,
                ResultToImport = finalResult

            };
            return routeInfo;
            var asd = 1;
        }



    }




}
