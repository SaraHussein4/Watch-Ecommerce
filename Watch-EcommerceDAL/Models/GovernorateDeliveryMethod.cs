using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_EcommerceDAL.Models
{
    public class GovernorateDeliveryMethod
    {
        public int Id { get; set; }
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public virtual Governorate Governorate { get; set; }

        [ForeignKey("DeliveryMethod")]
        public int DeliveryMethodId { get; set; }
        public virtual Deliverymethod DeliveryMethod { get; set; }

        public decimal DeliveryCost { get; set; }
    }
}
