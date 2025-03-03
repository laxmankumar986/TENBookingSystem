using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TENBookingSystem.DTO.Enum
{
    public class BookingsEnum
    {
        public enum BookingStatus
        {
            [Description("Booking Confirmed")]
            Confirmed = 1,
            [Description("Booking Processed")]
            Processed = 2,
            [Description("Booking Cancelled")]
            Cancelled = 3
        }
    }
}
