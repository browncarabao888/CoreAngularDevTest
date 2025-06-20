using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CoreAngularDevTest.Server.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AddressComponent> AddressComponents { get; set; }

    public virtual DbSet<Bound> Bounds { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Geometry> Geometries { get; set; }

    public virtual DbSet<ImageInfo> ImageInfos { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<LocationGeoInfo> LocationGeoInfos { get; set; }

    public virtual DbSet<Northeast> Northeasts { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Southwest> Southwests { get; set; }

    public virtual DbSet<Viewport> Viewports { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlite("Data Source=app.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("accounts");

            entity.Property(e => e.Emailaddress).HasColumnName("emailaddress");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.Passkey).HasColumnName("passkey");
            entity.Property(e => e.Status)
                .HasDefaultValue(0)
                .HasColumnName("status");
            entity.Property(e => e.UserName).HasColumnName("user_name");
        });

        modelBuilder.Entity<AddressComponent>(entity =>
        {
            entity.HasKey(e => e.ResultId);

            entity.ToTable("AddressComponent");

            entity.HasIndex(e => e.ResultId, "IX_AddressComponent_ResultId");

            entity.Property(e => e.ResultId).ValueGeneratedOnAdd();
            entity.Property(e => e.LongName).HasColumnName("Long_name");
            entity.Property(e => e.ShortName).HasColumnName("Short_name");

            entity.HasOne(d => d.Result).WithOne(p => p.AddressComponent)
                .HasForeignKey<AddressComponent>(d => d.ResultId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Bound>(entity =>
        {
            entity.HasIndex(e => e.NortheastId, "IX_Bounds_northeastId");

            entity.HasIndex(e => e.SouthwestId, "IX_Bounds_southwestId");

            entity.Property(e => e.NortheastId).HasColumnName("northeastId");
            entity.Property(e => e.SouthwestId).HasColumnName("southwestId");

            entity.HasOne(d => d.Northeast).WithMany(p => p.Bounds).HasForeignKey(d => d.NortheastId);

            entity.HasOne(d => d.Southwest).WithMany(p => p.Bounds).HasForeignKey(d => d.SouthwestId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_Id");
            entity.Property(e => e.CategoryName).HasColumnName("category_name");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(0)
                .HasColumnName("isactive");
        });

        modelBuilder.Entity<Geometry>(entity =>
        {
            entity.ToTable("Geometry");

            entity.HasIndex(e => e.BoundsId, "IX_Geometry_BoundsId");

            entity.HasIndex(e => e.LocationId, "IX_Geometry_LocationId");

            entity.HasIndex(e => e.ViewportId, "IX_Geometry_ViewportId");

            entity.Property(e => e.LocationType).HasColumnName("Location_type");

            entity.HasOne(d => d.Bounds).WithMany(p => p.Geometries).HasForeignKey(d => d.BoundsId);

            entity.HasOne(d => d.Location).WithMany(p => p.Geometries).HasForeignKey(d => d.LocationId);

            entity.HasOne(d => d.Viewport).WithMany(p => p.Geometries).HasForeignKey(d => d.ViewportId);
        });

        modelBuilder.Entity<ImageInfo>(entity =>
        {
            entity.ToTable("ImageInfo");

            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.Deleted)
                .HasDefaultValue(0)
                .HasColumnName("deleted");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Imagedata).HasColumnName("imagedata");
            entity.Property(e => e.Location).HasColumnName("location");
            entity.Property(e => e.Owner).HasColumnName("owner");
            entity.Property(e => e.Ownerrealname).HasColumnName("ownerrealname");
            entity.Property(e => e.PhotoId)
                .HasColumnType("NUMERIC")
                .HasColumnName("photoId");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Location");

            entity.Property(e => e.Lat).HasColumnName("lat");
            entity.Property(e => e.Lng).HasColumnName("lng");
        });

        modelBuilder.Entity<LocationGeoInfo>(entity =>
        {
            entity.ToTable("locationGeoInfo");
        });

        modelBuilder.Entity<Northeast>(entity =>
        {
            entity.ToTable("Northeast");

            entity.Property(e => e.Lat).HasColumnName("lat");
            entity.Property(e => e.Lng).HasColumnName("lng");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.ToTable("Result");

            entity.HasIndex(e => e.LocationGeoInfoId, "IX_Result_LocationGeoInfoId");

            entity.HasIndex(e => e.GeometryId, "IX_Result_geometryId");

            entity.Property(e => e.FormattedAddress).HasColumnName("formatted_address");
            entity.Property(e => e.GeometryId).HasColumnName("geometryId");
            entity.Property(e => e.PlaceId).HasColumnName("place_id");
            entity.Property(e => e.Types).HasColumnName("types");

            entity.HasOne(d => d.Geometry).WithMany(p => p.Results).HasForeignKey(d => d.GeometryId);

            entity.HasOne(d => d.LocationGeoInfo).WithMany(p => p.Results).HasForeignKey(d => d.LocationGeoInfoId);
        });

        modelBuilder.Entity<Southwest>(entity =>
        {
            entity.ToTable("Southwest");

            entity.Property(e => e.Lat).HasColumnName("lat");
            entity.Property(e => e.Lng).HasColumnName("lng");
        });

        modelBuilder.Entity<Viewport>(entity =>
        {
            entity.ToTable("Viewport");

            entity.HasIndex(e => e.NortheastId, "IX_Viewport_northeastId");

            entity.HasIndex(e => e.SouthwestId, "IX_Viewport_southwestId");

            entity.Property(e => e.NortheastId).HasColumnName("northeastId");
            entity.Property(e => e.SouthwestId).HasColumnName("southwestId");

            entity.HasOne(d => d.Northeast).WithMany(p => p.Viewports).HasForeignKey(d => d.NortheastId);

            entity.HasOne(d => d.Southwest).WithMany(p => p.Viewports).HasForeignKey(d => d.SouthwestId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
