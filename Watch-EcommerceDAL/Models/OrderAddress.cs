using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_EcommerceDAL.Models
{
    public class OrderAddress
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public string Street { get; set; }
        public virtual Governorate Governorate { get; set; }

    }
}

