using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_EcommerceDAL.Models
{
    public class Deliverymethod
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
            public string Description { get; set; }
            public string DeliveryTime { get; set; }
            public decimal Cost { get; set; }
        }
    
}
