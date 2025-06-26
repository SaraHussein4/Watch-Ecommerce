﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    
        // Navigation Property
        public virtual  ICollection<Product> Products { get; set; }
    }
}
