using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Models;

public partial class ProductDbContext : DbContext
{
    public ProductDbContext()
    {
    }

    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Clarity> Clarities { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Cut> Cuts { get; set; }

    public virtual DbSet<Eligibility> Eligibilities { get; set; }

    public virtual DbSet<Fluorescence> Fluorescences { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Lab> Labs { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Polish> Polishes { get; set; }

    public virtual DbSet<Symmetry> Symmetries { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=C:\\Projects\\LTProject\\ProductService\\ProductService.Database\\ProductDb.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clarity>(entity =>
        {
            entity.ToTable("Clarity");

            entity.HasIndex(e => e.Name, "IX_Clarity_Name").IsUnique();
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.ToTable("Color");

            entity.HasIndex(e => e.Name, "IX_Color_Name").IsUnique();
        });

        modelBuilder.Entity<Cut>(entity =>
        {
            entity.ToTable("Cut");

            entity.HasIndex(e => e.Name, "IX_Cut_Name").IsUnique();
        });

        modelBuilder.Entity<Eligibility>(entity =>
        {
            entity.ToTable("Eligibility");

            entity.HasIndex(e => e.Name, "IX_Eligibility_Name").IsUnique();
        });

        modelBuilder.Entity<Fluorescence>(entity =>
        {
            entity.ToTable("Fluorescence");

            entity.HasIndex(e => e.Name, "IX_Fluorescence_Name").IsUnique();
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");

            entity.HasIndex(e => e.ItemCode, "IX_Item_ItemCode").IsUnique();

            entity.Property(e => e.DiscountPercent).HasDefaultValue(0.0);
            entity.Property(e => e.FinalPrice).HasComputedColumnSql();

            entity.HasOne(d => d.Clarity).WithMany(p => p.Items).HasForeignKey(d => d.ClarityId);

            entity.HasOne(d => d.Color).WithMany(p => p.Items).HasForeignKey(d => d.ColorId);

            entity.HasOne(d => d.Cut).WithMany(p => p.Items).HasForeignKey(d => d.CutId);

            entity.HasOne(d => d.Eligibility).WithMany(p => p.Items).HasForeignKey(d => d.EligibilityId);

            entity.HasOne(d => d.Fluorescence).WithMany(p => p.Items).HasForeignKey(d => d.FluorescenceId);

            entity.HasOne(d => d.Lab).WithMany(p => p.Items).HasForeignKey(d => d.LabId);

            entity.HasOne(d => d.Location).WithMany(p => p.Items).HasForeignKey(d => d.LocationId);

            entity.HasOne(d => d.Polish).WithMany(p => p.Items).HasForeignKey(d => d.PolishId);

            entity.HasOne(d => d.Symmetry).WithMany(p => p.Items).HasForeignKey(d => d.SymmetryId);

            entity.HasOne(d => d.Type).WithMany(p => p.Items).HasForeignKey(d => d.TypeId);
        });

        modelBuilder.Entity<Lab>(entity =>
        {
            entity.ToTable("Lab");

            entity.HasIndex(e => e.Name, "IX_Lab_Name").IsUnique();
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Location");

            entity.HasIndex(e => e.Name, "IX_Location_Name").IsUnique();
        });

        modelBuilder.Entity<Polish>(entity =>
        {
            entity.ToTable("Polish");

            entity.HasIndex(e => e.Name, "IX_Polish_Name").IsUnique();
        });

        modelBuilder.Entity<Symmetry>(entity =>
        {
            entity.ToTable("Symmetry");

            entity.HasIndex(e => e.Name, "IX_Symmetry_Name").IsUnique();
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.ToTable("Type");

            entity.HasIndex(e => e.Name, "IX_Type_Name").IsUnique();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
