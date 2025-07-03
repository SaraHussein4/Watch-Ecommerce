using ECommerce.Core.Enumerators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceDAL.Models;

namespace ECommerce.Core.model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity {  get; set; }
        public string Status {  get; set; }
        public string GenderCategory {  get; set; }
        public bool WaterResistance {  get; set; }
        public int WarrentyYears {  get; set; }

        public string Colors { get; set; }
        public string Sizes { get; set; }

        // Foreign Key
        public int ProductBrandId { get; set; }
        public int CategoryId { get; set; }
        
        // Navigation Property
        public virtual ProductBrand? ProductBrand { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<Image>? Images { get; set; }
        public virtual ICollection<Favourite>? Users { get; set; }

        //public virtual ICollection<ProductSize>? Sizes { get; set; }
        //public virtual ICollection<ProductColor>? Colors { get; set; }


    }
}
