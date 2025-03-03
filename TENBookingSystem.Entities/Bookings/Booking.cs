using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TENBookingSystem.Entities.Inventorys;
using TENBookingSystem.Entities.Members;

namespace TENBookingSystem.Entities.Bookings
{
    public class Booking
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int InventoryId { get; set; }
        public DateOnly BookingDate { get; set; }
        public DateTime BokkingTimeStamp { get; set; }        
        public Guid BookingReference { get; set; } = Guid.NewGuid();
        public int BookingStatus { get; set; }
        public virtual Inventory Inventory { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;
    }
}
