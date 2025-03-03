using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TENBookingSystem.DTO.Booking
{
    public class BookingListDto
    {
        public int BookingId { get; set; }
        public string MemberName { get; set; }
        public string Title { get; set; }
        public string BookingDate { get; set; }
        public string BookingStatus { get; set; }
    }
}
