using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.model
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool isPrimary {  get; set; }

        // Foreign Key
        public int ProductId {  get; set; }

        // Navigation Property

        public virtual Product Product { get; set; }

    }
}
