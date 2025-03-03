using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TENBookingSystem.DTO.Booking
{
    public class BookingDto
    {
        public int MemberId { get; set; }
        public int InventoryId { get; set; }
        public DateTime BookingDate { get; set; }
        public int BookingStatus { get; set; }

    }
}
