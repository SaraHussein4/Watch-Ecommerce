using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceDAL.Models;

namespace ECommerce.Core.model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date {  get; set; } = DateTime.Now;
        public string Status { get; set; }
        public decimal Amount {  get; set; }

        public OrderAddress OrderAddress { get; set; }
        public int DeliveryMethodId { get; set; }
        public virtual Deliverymethod Deliverymethod { get; set; }

        public decimal SubTotal { get; set; }

        //payment
        public string PaymentInterntId { get; set; }

        // Foreign Key
        public string UserId { get; set; }
        
        // Navigation Property
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
