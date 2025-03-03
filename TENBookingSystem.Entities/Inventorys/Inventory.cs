using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TENBookingSystem.Entities.Bookings;

namespace TENBookingSystem.Entities.Inventorys
{
    public class Inventory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descritpion { get; set; }
        public int RemainingCount { get; set; }
        public DateTime ExpireDate { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
