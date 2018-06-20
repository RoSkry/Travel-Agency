using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    class TourInfo
    {
       public int Id { get; set; }
        public string TourCountry { get; set; }
        public string TourCity{ get; set; }
        public DateTime TourBegin { get; set; }
        public DateTime TourEnd { get; set; }         
        public int MaxSeats { get; set; }
        public int CurrentSeats { get; set; }
        public double Price { get; set; }      
        public int DiscountId{ get; set; }   
    }
}
