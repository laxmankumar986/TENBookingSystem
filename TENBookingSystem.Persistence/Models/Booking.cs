using System;
using System.Collections.Generic;

namespace TENBookingSystem.Persistence.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int Memberid { get; set; }

    public int Inventoryid { get; set; }

    public DateOnly Bookingdate { get; set; }

    public DateTime Bookingtimestamp { get; set; }

    public Guid Bookingreference { get; set; }

    public int Bookingstatus { get; set; }

    public virtual Inventory Inventory { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
