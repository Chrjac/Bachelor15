using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelroute
{
    public class Barrier
    {
       
        public Barrier(string name, int priceSmall, int priceTruck)
        {
       
            this.Name = name;
            this.PriceSmallCar = priceSmall;
            this.PriceTruck = priceTruck;
        }
        
        public String Name { get; set; }
        public int PriceSmallCar { get; set; }
        public int PriceTruck { get; set; }
    }
}
