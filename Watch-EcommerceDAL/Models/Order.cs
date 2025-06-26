using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date {  get; set; }
        public string Status { get; set; }
        public decimal Amount {  get; set; }
        
        // Foreign Key
        public string UserId { get; set; }
        
        // Navigation Property
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
