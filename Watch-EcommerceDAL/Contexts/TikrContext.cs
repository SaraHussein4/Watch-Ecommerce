using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceDAL.Models;

namespace ECommerce.Core.model
{
    public class TikrContext : IdentityDbContext<User>
    {
        public TikrContext(DbContextOptions<TikrContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Address
            builder.Entity<Address>().HasKey(a => a.Id);

            builder.Entity<Address>().Property(a => a.Street).HasMaxLength(100);
            builder.Entity<Address>().Property(a => a.State).HasMaxLength(100);

            builder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
            
            #region Category
            builder.Entity<Category>().HasKey(a => a.Id);
            builder.Entity<Category>().Property(a => a.Name).HasMaxLength(100);

            #endregion
           
            #region Favourite
            builder.Entity<Favourite>().HasKey(f => new {f.UserId, f.ProductId});
            builder.Entity<Favourite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Products)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Favourite>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Users)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Image
            builder.Entity<Image>().HasKey(i => i.Id);
            builder.Entity<Image>().Property(i => i.Url).HasMaxLength(100);
            builder.Entity<Image>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
            
            #region Order
            builder.Entity<Order>().HasKey(o => o.Id);
            builder.Entity<Order>().Property(o => o.Status).HasMaxLength(100);
            builder.Entity<Order>().Property(o => o.Amount).HasColumnType("decimal(18,2)");

            builder.Entity<Order>().Property(o => o.Date).HasColumnType("DATETIME2");

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>(builder =>
            {
                builder.Property(o => o.Status).HasConversion<string>();

                
                builder.OwnsOne(o => o.OrderAddress, oa =>
                {
                    oa.WithOwner();

                    oa.Property(a => a.FirstName).IsRequired();
                    oa.Property(a => a.LastName).IsRequired();
                    oa.Property(a => a.City).IsRequired();
                    oa.Property(a => a.Street).IsRequired();
                    oa.Property(a => a.GovernorateId).IsRequired();
                });

                builder.HasOne(o => o.Deliverymethod)
                       .WithMany()
                       .HasForeignKey(o => o.DeliveryMethodId)
                       .OnDelete(DeleteBehavior.NoAction);

                builder.Property(o => o.Amount).HasColumnType("decimal(18,2)");
            });
            #endregion

            #region OrderItem
            builder.Entity<OrderItem>().HasKey(oi => oi.Id);
            builder.Entity<OrderItem>().Property(oi => oi.Amount).HasColumnType("decimal(18,2)");
            builder.Entity<OrderItem>().Property(oi => oi.Price).HasColumnType("decimal(18,2)");

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            #endregion

            #region Product
            builder.Entity<Product>().HasKey(p => p.Id);

            builder.Entity<Product>().Property(p => p.Name).HasMaxLength(100);
            builder.Entity<Product>().Property(p => p.Description).HasMaxLength(200);
            builder.Entity<Product>().Property(p => p.Status).HasMaxLength(100);

            builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");

            builder.Entity<Product>().Property(p => p.GenderCategory).HasConversion<string>().HasMaxLength(20);


            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasOne(p => p.ProductBrand)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ProductBrandId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region ProductBrand
                builder.Entity<ProductBrand>().HasKey(pb => pb.Id);
                builder.Entity<ProductBrand>().Property(pb => pb.Name).HasMaxLength(100);
            #endregion

            #region User
            builder.Entity<User>().Property(u => u.Name).HasMaxLength(100);
            #endregion


            //#region Color
            //builder.Entity<Color>().HasKey(a => a.Id);
            //builder.Entity<Color>().Property(a => a.Name).HasMaxLength(100);
            //#endregion

            //#region Size
            //builder.Entity<Size>().HasKey(a => a.Id);
            //builder.Entity<Size>().Property(a => a.Name).HasMaxLength(100);
            //#endregion


            //#region ProductColor
            //builder.Entity<ProductColor>().HasKey(pc => new { pc.ProductId, pc.ColorId});
            //builder.Entity<ProductColor>()
            //    .HasOne(pc => pc.Product)
            //    .WithMany(p => p.Colors)
            //    .HasForeignKey(pc => pc.ProductId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<ProductColor>()
            //    .HasOne(pc => pc.Color)
            //    .WithMany(c => c.Products)
            //    .HasForeignKey(pc => pc.ColorId)
            //    .OnDelete(DeleteBehavior.NoAction);
            //#endregion

            //#region ProductSize
            //builder.Entity<ProductSize>().HasKey(ps => new { ps.ProductId, ps.SizeId });
            //builder.Entity<ProductSize>()
            //    .HasOne(ps => ps.Product)
            //    .WithMany(p => p.Sizes)
            //    .HasForeignKey(ps => ps.ProductId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<ProductSize>()
            //    .HasOne(ps => ps.Size)
            //    .WithMany(s => s.Products)
            //    .HasForeignKey(ps => ps.SizeId)
            //    .OnDelete(DeleteBehavior.NoAction);
            //#endregion
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Favourite> Favourites{ get; set; }
        public DbSet<Image> Images{ get; set; }
        public DbSet<OrderItem> OrderItems{ get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands{ get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Governorate> Governorates { get; set; }

        public  DbSet<Deliverymethod> Deliverymethods { get; set; }
        public DbSet<GovernorateDeliveryMethod> GovernorateDeliveryMethods { get; set; }

        //public DbSet<Color> Colors { get; set; }
        //public DbSet<Size> Sizes { get; set; }
        //public DbSet<ProductColor> ProductColors { get; set; }
        //public DbSet<ProductSize> productSizes { get; set; }


    }
}
