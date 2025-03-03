using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TENBookingSystem.Entities.Bookings;
using TENBookingSystem.Entities.Inventorys;
using TENBookingSystem.Entities.Members;
using TENBookingSystem.Persistence.EntityConfiguration;

namespace TENBookingSystem.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Inventory> Inventorys { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MemberMappingConfiguration());
            builder.ApplyConfiguration(new InventoryMappingConfiguration());
            builder.ApplyConfiguration(new BookingMappingConfiguration());
        }
    }
}
