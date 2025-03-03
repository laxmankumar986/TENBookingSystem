using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TENBookingSystem.Entities.Bookings;


namespace TENBookingSystem.Persistence.EntityConfiguration
{
    public class BookingMappingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(e => e.Id).HasName("booking_pkey");

            builder.ToTable("booking");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.BookingDate).HasColumnName("bookingdate");
            builder.Property(e => e.BookingReference)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("bookingreference");
            builder.Property(e => e.BookingStatus).HasColumnName("bookingstatus");
            builder.Property(e => e.BokkingTimeStamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("bookingtimestamp");
            builder.Property(e => e.InventoryId).HasColumnName("inventoryid");
            builder.Property(e => e.MemberId).HasColumnName("memberid");

            builder.HasOne(d => d.Inventory).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.InventoryId)
                .HasConstraintName("fk_inventory");

            builder.HasOne(d => d.Member).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("fk_member");
        }
    }
}
