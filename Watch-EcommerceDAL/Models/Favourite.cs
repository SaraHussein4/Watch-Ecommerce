using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.model
{
    public class Favourite
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }

        // Navigation Property
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
