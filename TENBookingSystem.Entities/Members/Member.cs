using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TENBookingSystem.Entities.Bookings;

namespace TENBookingSystem.Entities.Members
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurnName { get; set; }
        public int BookingCount { get; set; }
        public DateTime DateJoined { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
