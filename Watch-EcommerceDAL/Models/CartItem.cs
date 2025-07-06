using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_EcommerceDAL.Models
{
    public class CartItem
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        //public string PaymentInterntId { get; set; }
        //public string ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
    }
}
