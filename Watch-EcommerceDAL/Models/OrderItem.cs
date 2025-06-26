using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.model
{
    public class OrderItem 
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Amount {  get; set; }

        // Foreign Key
        public int ProductId {  get; set; }
        public int OrderId { get; set; }  
        // Navigation Property 
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }

    }
}
