using System;
using System.Collections.Generic;

namespace TENBookingSystem.Persistence.Models;

public partial class Inventory
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Remainingcount { get; set; }

    public DateTime Expiredate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
