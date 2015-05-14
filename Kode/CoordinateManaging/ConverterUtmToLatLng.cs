using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateManaging
{
    public class ConverterUtmToLatLng
    {
        public List<Tuple<double, double>> ConvertAll(List<Tuple<double, double>> decompressedGeometry)
        {
            var listOfLatLng = new List<Tuple<double, double>>();


            for (int i = 0; i < decompressedGeometry.Count(); i++)
            {
                var converted = ToLatLon(decompressedGeometry[i].Item1, decompressedGeometry[i].Item2, "33N");
                listOfLatLng.Add(converted);
            }
            /* var reducedListOfLatLng = new List<double[]>();
             reducedListOfLatLng = ReduceCordinateList(listOfLatLng);*/


            return listOfLatLng;


        }
        public static List<Double[]> ReduceCordinateList(List<double[]> listOfLatLng)
        {
            var numberOfCoordinates = listOfLatLng.Count();
            var difference = (numberOfCoordinates / 8) + 1;
            var reducedListOfLatLng = new List<double[]>();

            for (int i = 0; i < numberOfCoordinates; i = i + difference)
            {
                reducedListOfLatLng.Add(listOfLatLng[i]);

            }

            return reducedListOfLatLng;

        }
        public static Tuple<double, double> ToLatLon(double utmX, double utmY, string utmZone)
        {
            var u = new List<double[]>();
            bool isNorthHemisphere = utmZone.Last() >= 'N';

            var diflat = 0; //-0.00066286966871111111111111111111111111;
            var diflon = 0;//-0.0003868060578;

            var zone = int.Parse(utmZone.Remove(utmZone.Length - 1));
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            var longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            var latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
            //  Debug.WriteLine("lat " + latitude + " long " + longitude);

            var listOfLatLng = new List<double[]>();
            double[] arr = new double[2] { latitude, longitude };
            var result = new Tuple<double, double>(arr[0], arr[1]);
            u.Add(arr);
            return result;
        }

    }
}