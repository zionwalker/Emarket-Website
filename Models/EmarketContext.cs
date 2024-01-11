using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Emarket_Website.Models;

public partial class EmarketContext : DbContext
{
    public EmarketContext()
    {
    }

    public EmarketContext(DbContextOptions<EmarketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApplicationStatus> ApplicationStatuses { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<CartItemStatuss> CartItemStatusses { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemCategory> ItemCategories { get; set; }

    public virtual DbSet<ItemConfiguration> ItemConfigurations { get; set; }

    public virtual DbSet<ItemImage> ItemImages { get; set; }

    public virtual DbSet<ItemPrice> ItemPrices { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatuss> OrderStatusses { get; set; }

    public virtual DbSet<Ordereditem> Ordereditems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ReviewShop> ReviewShops { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SellerApplication> SellerApplications { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<ShopCategory> ShopCategories { get; set; }

    public virtual DbSet<ShopEntry> ShopEntries { get; set; }

    public virtual DbSet<ShopEntryVariation> ShopEntryVariations { get; set; }

    public virtual DbSet<ShopKeeper> ShopKeepers { get; set; }

    public virtual DbSet<SoldItem> SoldItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VariationType> VariationTypes { get; set; }

    public virtual DbSet<VariationTypePossibleValue> VariationTypePossibleValues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DefaultnConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ShoppingCartItems");

            entity.HasOne(d => d.CartItemStatus).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_CartItemStatusses");

            entity.HasOne(d => d.Item).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Items");

            entity.HasOne(d => d.Shop).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Shops");

            entity.HasOne(d => d.User).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Users");
        });

        modelBuilder.Entity<CartItemStatuss>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CartItemStatus");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Products");

            entity.HasOne(d => d.ItemCat).WithMany(p => p.Items)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Items_ItemCategories");
        });

        modelBuilder.Entity<ItemCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProductCatagories");
        });

        modelBuilder.Entity<ItemConfiguration>(entity =>
        {
            entity.HasOne(d => d.Item).WithMany(p => p.ItemConfigurations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemConfigurations_Items");

            entity.HasOne(d => d.VarOpt).WithMany(p => p.ItemConfigurations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemConfigurations_VariationTypePossibleValues");
        });

        modelBuilder.Entity<ItemImage>(entity =>
        {
            entity.HasOne(d => d.Item).WithMany(p => p.ItemImages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemImages_Items");
        });

        modelBuilder.Entity<ItemPrice>(entity =>
        {
            entity.HasOne(d => d.Item).WithMany(p => p.ItemPrices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemPrices_Items");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.OrderStat).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_OrderStatusses");

            entity.HasOne(d => d.Shop).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Shops");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderStatuss>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OrderStatus");
        });

        modelBuilder.Entity<Ordereditem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OrderLine");

            entity.HasOne(d => d.Item).WithMany(p => p.Ordereditems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ordereditems_Items");

            entity.HasOne(d => d.Order).WithMany(p => p.Ordereditems)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Ordereditems_Orders");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Payments_1");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Orders");

            entity.HasOne(d => d.PayTypet).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_PaymentTypes");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PaymentType");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasOne(d => d.Item).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_Items");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_Users");
        });

        modelBuilder.Entity<ReviewShop>(entity =>
        {
            entity.HasOne(d => d.Shop).WithMany(p => p.ReviewShops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewShops_Shops");

            entity.HasOne(d => d.User).WithMany(p => p.ReviewShops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewShops_Users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Rname).HasDefaultValueSql("(N'Buyer')");
        });

        modelBuilder.Entity<SellerApplication>(entity =>
        {
            entity.HasOne(d => d.ApplicationStatus).WithMany(p => p.SellerApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellerApplications_ApplicationStatus");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.SellerApplicationApprovedByNavigations).HasConstraintName("FK_SellerApplications_Users1");

            entity.HasOne(d => d.User).WithMany(p => p.SellerApplicationUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellerApplications_Users");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasOne(d => d.City).WithMany(p => p.Shops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shops_Cities");

            entity.HasOne(d => d.ShopCat).WithMany(p => p.Shops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shops_ShopCategories");

            entity.HasOne(d => d.User).WithMany(p => p.Shops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shops_Users");
        });

        modelBuilder.Entity<ShopCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ShopCatagories");
        });

        modelBuilder.Entity<ShopEntry>(entity =>
        {
            entity.HasOne(d => d.Item).WithMany(p => p.ShopEntries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopEntries_Items");

            entity.HasOne(d => d.Shop).WithMany(p => p.ShopEntries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopEntries_Shops");
        });

        modelBuilder.Entity<ShopEntryVariation>(entity =>
        {
            entity.HasOne(d => d.ShopEntry).WithMany(p => p.ShopEntryVariations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopEntryVariations_ShopEntries");

            entity.HasOne(d => d.Var).WithMany(p => p.ShopEntryVariations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopEntryVariations_VariationTypes");
        });

        modelBuilder.Entity<ShopKeeper>(entity =>
        {
            entity.HasOne(d => d.Shop).WithMany(p => p.ShopKeepers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopKeepers_Shops");

            entity.HasOne(d => d.User).WithMany(p => p.ShopKeepers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopKeepers_Users");
        });

        modelBuilder.Entity<SoldItem>(entity =>
        {
            entity.HasOne(d => d.ShopEntry).WithMany(p => p.SoldItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SoldItems_ShopEntries");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.Password).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.City).WithMany(p => p.Users).HasConstraintName("FK_Users_Cities");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAddresses_Users");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Roles");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Users");
        });

        modelBuilder.Entity<VariationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Variations");

            entity.HasOne(d => d.ItemCat).WithMany(p => p.VariationTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VariationTypes_ItemCategories");
        });

        modelBuilder.Entity<VariationTypePossibleValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_VariationOption");

            entity.HasOne(d => d.Var).WithMany(p => p.VariationTypePossibleValues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VariationTypePossibleValues_VariationTypes");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
