using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceDAL.Models;

namespace ECommerce.Core.model
{
    public class User : IdentityUser
    {
        public string Name {  get; set; }

        public int? GovernorateId { get; set; }
        public virtual Governorate Governorate { get; set; }
        // Navigation Property
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Favourite> Products { get; set; }

    }
}
