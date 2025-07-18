﻿using Watch_Ecommerce.DTOs.Product;
using Watch_Ecommerce.DTOS.Product;

namespace Watch_Ecommerce.DTOS.Category
{
    public class CategoryReadDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public virtual ICollection<DisplayProductDTO> Products { get; set; }

    }
}
