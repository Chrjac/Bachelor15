using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelroute
{
    public class Point
    {

        public Point(string name, string xcoordinate, string ycoordinate)
        {

            this.Name = name;
            this.Xcoordinate = xcoordinate;
            this.Ycoordinate = ycoordinate;
        }
        public Point(string name)
        {

            this.Name = name;
     
        }
       
       
        public String Name { get; set; }
        public String Xcoordinate { get; set; }
        public String Ycoordinate { get; set; }
    }
}
