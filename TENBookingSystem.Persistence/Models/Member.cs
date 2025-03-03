using System;
using System.Collections.Generic;

namespace TENBookingSystem.Persistence.Models;

public partial class Member
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surnname { get; set; } = null!;

    public int Bookingcount { get; set; }

    public DateTime Datejoined { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
