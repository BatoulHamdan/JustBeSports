using JustBeSports.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace JustBeSports.Core.Context
{
    public partial class JustBeSportsDbContext : DbContext
    {
        public JustBeSportsDbContext(DbContextOptions<JustBeSportsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ProductVariant> ProductVariants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__CartItem__3214EC07377DA0E4");

                entity.ToTable("CartItem");

                entity.Property(e => e.SessionId).HasMaxLength(255);

                entity.HasOne(d => d.Order)
                      .WithMany(p => p.CartItems)
                      .HasForeignKey(d => d.OrderId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_CartItem_Order");

                entity.HasOne(d => d.Product)
                      .WithMany(p => p.CartItems)
                      .HasForeignKey(d => d.ProductId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_CartItem_Product");

                entity.HasOne(d => d.ProductVariant)
                      .WithMany(p => p.CartItems)
                      .HasForeignKey(d => d.ProductVariantId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_CartItem_ProductVariant");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07A6AD64B2");

                entity.ToTable("Order");

                entity.Property(e => e.FirstName).HasMaxLength(255);
                entity.Property(e => e.FullAddress).HasMaxLength(500);
                entity.Property(e => e.Governate).HasMaxLength(255);
                entity.Property(e => e.InstagramAccount).HasMaxLength(255);
                entity.Property(e => e.LastName).HasMaxLength(255);
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.PhoneNumber).HasMaxLength(50);
                entity.Property(e => e.SessionId).HasMaxLength(255);
                entity.Property(e => e.Status)
                      .HasMaxLength(50)
                      .HasDefaultValue("Pending");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Color).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Category)
                      .WithMany(p => p.Products)
                      .HasForeignKey(d => d.CategoryId)
                      .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.Property(e => e.Url).HasMaxLength(255);

                entity.HasOne(d => d.Product)
                      .WithMany(p => p.ProductImages)
                      .HasForeignKey(d => d.ProductId)
                      .HasConstraintName("FK_ProductImages_Product");
            });

            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ProductV__3214EC0780B4A352");

                entity.ToTable("ProductVariant");

                entity.Property(e => e.Size).HasMaxLength(20);

                entity.HasOne(d => d.Product)
                      .WithMany(p => p.ProductVariants)
                      .HasForeignKey(d => d.ProductId)
                      .HasConstraintName("FK_ProductVariant_Product");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
