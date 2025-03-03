using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TENBookingSystem.Entities.Inventorys;
using TENBookingSystem.Entities.Members;


namespace TENBookingSystem.Persistence.EntityConfiguration
{
    public class InventoryMappingConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> entity)
        {
            entity.HasKey(e => e.Id).HasName("inventory_pkey");

            entity.ToTable("inventory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descritpion).HasColumnName("description");
            entity.Property(e => e.ExpireDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expiredate");
            entity.Property(e => e.RemainingCount)
                .HasDefaultValue(0)
                .HasColumnName("remainingcount");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        }
    }
}
