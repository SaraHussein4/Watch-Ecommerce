using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.model
{
    public class Address
    {
        public int Id {  get; set; }
        public int BuildingNumber { get; set; } 
        public string Street {  get; set; }
        public string State { get; set; }
        public bool IsDefault { get; set; }

        // Foreign Key
        public string UserId {  get; set; }

        // Navigation Propety
        public virtual User User { get; set; }

    }
}
