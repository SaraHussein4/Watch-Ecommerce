using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_EcommerceDAL.Models
{
    public class CustomerBasket
    {
        public CustomerBasket(string id)
        {
            Id = id;
            Items = new List<CartItem>();
        }
        
        public string Id { get; set; }
        public List<CartItem> Items { get; set; }
    }
}
