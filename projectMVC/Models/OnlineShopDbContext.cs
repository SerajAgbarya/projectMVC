using Microsoft.EntityFrameworkCore;
using projectMVC.Models;
using projectMVC.Models.Ref;

using System.Collections.Generic;

public class OnlineShopDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options) : base(options) { }

    public DbSet<RefDataModel> ref_data { get; set; }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<UserPermissionModel> Users_permission { get; set; }

    public DbSet<ProductCategoresModel> Product_categories { get; set; }
    public DbSet<ProductCategoryAttributesModel> Product_category_attributes { get; set; }

    public DbSet<ProductsModel> Products { get; set; }
    public DbSet<ProductAttributesModel> Product_attributes { get; set; }


    public DbSet<OrderModel> Orders { get; set; }
    public DbSet<OrderProductsModel> Order_products { get; set; }

    public DbSet<DiscountModel> Discounts { get; set; }

    public DbSet<CartModel> Cart { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategoryAttributesModel>()
            .HasKey(entity => new { entity.CategoryName, entity.AttributeName });

        modelBuilder.Entity<RefDataModel>()
           .HasKey(entity => new { entity.RefName, entity.RefValue });

        modelBuilder.Entity<OrderProductsModel>()
          .HasKey(entity => new { entity.orderId, entity.productId });

        modelBuilder.Entity<CartModel>()
         .HasKey(entity => new { entity.UserId, entity.ProductId });
    }

}
