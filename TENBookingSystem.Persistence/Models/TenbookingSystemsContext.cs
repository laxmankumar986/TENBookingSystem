using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TENBookingSystem.Persistence.Models;

public partial class TenbookingSystemsContext : DbContext
{
    public TenbookingSystemsContext()
    {
    }

    public TenbookingSystemsContext(DbContextOptions<TenbookingSystemsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=TENBookingSystems;Pooling=true;Connection Lifetime=0;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("booking_pkey");

            entity.ToTable("booking");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bookingdate).HasColumnName("bookingdate");
            entity.Property(e => e.Bookingreference)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("bookingreference");
            entity.Property(e => e.Bookingstatus).HasColumnName("bookingstatus");
            entity.Property(e => e.Bookingtimestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("bookingtimestamp");
            entity.Property(e => e.Inventoryid).HasColumnName("inventoryid");
            entity.Property(e => e.Memberid).HasColumnName("memberid");

            entity.HasOne(d => d.Inventory).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.Inventoryid)
                .HasConstraintName("fk_inventory");

            entity.HasOne(d => d.Member).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.Memberid)
                .HasConstraintName("fk_member");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("inventory_pkey");

            entity.ToTable("inventory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Expiredate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expiredate");
            entity.Property(e => e.Remainingcount)
                .HasDefaultValue(0)
                .HasColumnName("remainingcount");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("member_pkey");

            entity.ToTable("member");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bookingcount)
                .HasDefaultValue(0)
                .HasColumnName("bookingcount");
            entity.Property(e => e.Datejoined)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datejoined");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Surnname)
                .HasMaxLength(255)
                .HasColumnName("surnname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
