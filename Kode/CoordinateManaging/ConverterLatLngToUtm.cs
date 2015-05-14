using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateManaging
{
    public class ConverterLatLngToUtm
    {

        public void latlongToUTM(double latitude, double longitude)
        {
          

    var DatumEqRad = new double[14]{6378137.0, 6378137.0, 6378137.0, 6378135.0, 6378160.0, 6378245.0, 6378206.4, 6378388.0, 6378388.0, 6378249.1, 6378206.4, 6377563.4, 6377397.2, 6377276.3};

    var DatumFlat =  new double[]{298.2572236, 298.2572236, 298.2572215, 298.2597208, 298.2497323, 298.2997381, 294.9786982, 296.9993621, 296.9993621, 293.4660167, 294.9786982, 299.3247788,
    299.1527052, 300.8021499};

   var Item = 0;

    //skala sentralmeridian (CM)

    var k0 = 0.9996;

    //ekvatorial radius, meter

   var a = DatumEqRad[Item];

    //polar utflatning

   var f = 1 / DatumFlat[Item];

    //polar akse

   var b = a * (1 - f);

    //eksentrisitet

    var e = Math.Sqrt(1 - b * b / a * a);

    //konvertere grader til radianer

    var drad = Math.PI / 180;

    //latitude (breddegrad) i grader

    var latd = 0.0;

    //lattitude (nord +, sør -)

    var phi = 0.0;

   var e0 = e / Math.Sqrt(1 - e * e);

    var N = a / Math.Sqrt(1 - Math.Pow((e * (Math.Sin(phi))), 2));

    var T = Math.Pow(Math.Tan(phi), 2);

    var C = Math.Pow(e * Math.Cos(phi), 2);

    //longitude (lengdegrad)

   var lng = 0.0;

    //longitude av sentralmeridianen

    var lngd0 = 0.0;

    //longitude i grader

    var lngd = 0.0;

   var M = 0.0;

    //x koordinat

  var  x = 0.0;

    //y koordinat

    var y = 0.0;

   //var k = 1;

    //utm sone


    //sone sentralmeridian

    var zcm = 0.0;

//.....

  

    k0 = 0.9996;

    b = a * (1 - f);

    e = Math.Sqrt(1 - (b / a) * (b / a));

    var latd0 = latitude;
    lngd0 = longitude;

    lngd = lngd0;
    latd = latd0;

    //konvertere latitude til radianer

    phi = latd * drad;

    //konvertere longitude til radianer

    lng = lngd * drad;

    var utmz = 33.0;
   var latz = 0.0;

    if (latd > -80 && latd < 72) {

        latz =( Math.Floor((latd + 80) / 8)) + 2;

    }

    if (latd > 72 && latd < 84) {

        latz = 21;

    }

    if (latd > 84) {

        latz = 23;

    }

    zcm = 3 + (6 * (utmz - 1)) - 180;

    e0 = e / Math.Sqrt(1 - (e * e));

    var esq = (1 - (b / a) * (b / a));

    var e0sq = e * e / (1 - e * e);

    N = a / (Math.Sqrt(1 - (Math.Pow((e * Math.Sin(phi)), 2))));

    T = Math.Pow(Math.Tan(phi), 2);

    C = e0sq * (Math.Pow(Math.Cos(phi), 2));

   var A = (lngd - zcm) * drad * Math.Cos(phi);

    M = phi * (1 - esq * (1 / 4 + esq * (3 / 64 + 5 * esq / 256)));

    M = M - (Math.Sin(2 * phi)) * (esq * (3 / 8 + esq * (3 / 32 + 45 * esq / 1024)));

    M = M + (Math.Sin(4 * phi)) * (esq * esq * (15 / 256 + esq * 45 / 1024));

    M = M - (Math.Sin(6 * phi)) * (esq * esq * esq * (35 / 3072));

    M = M * a;

    var M0 = 0;

    //kalkuler UTM verdier

    x = k0 * N * A * (1 + A * A * ((1 - T + C) / 6 + A * A * (5 - 18 * T + T * T + 72 * C - 58 * e0sq) / 120));

    x = x + 500000;

    y = k0 * (M - M0 + (N * Math.Tan(phi) * (A * A * (1 / 2 + A * A * ((5 - T + 9 * C + 4 * C * C) / 24 + A * A * (61 - 58 * T + T * T + 600 * C - 330 * e0sq) / 720)))));

    //yg = y + 10000000;

    if (y < 0) {

        y = 10000000 + y;

    }


    var xkord = 10 * (x) / 10;

   var ykord = 10 * y / 10;



            Debug.WriteLine("x: " + x + " y: " + y);

        }
    }
}

