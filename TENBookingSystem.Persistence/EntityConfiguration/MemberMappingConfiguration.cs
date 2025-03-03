using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TENBookingSystem.Entities.Members;

namespace TENBookingSystem.Persistence.EntityConfiguration
{
    public class MemberMappingConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> entity)
        {
            entity.HasKey(e => e.Id).HasName("member_pkey");

            entity.ToTable("member");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingCount)
                .HasDefaultValue(0)
                .HasColumnName("bookingcount");
            entity.Property(e => e.DateJoined)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datejoined");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.SurnName)
                .HasMaxLength(255)
                .HasColumnName("surnname");
        }
    }
}
