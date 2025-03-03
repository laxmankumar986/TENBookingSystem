using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TENBookingSystem.Application.Core;
using TENBookingSystem.DTO.Booking;
using TENBookingSystem.DTO.Uploads;

namespace TENBookingSystem.Application.Commands.Bookings
{
    public class BookingListCommand : IRequest<Result<List<BookingListDto>>>
    {   
    }
}
