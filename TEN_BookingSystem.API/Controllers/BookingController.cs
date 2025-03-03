using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TENBookingSystem.Application.Commands.Bookings;
using TENBookingSystem.Application.Core;
using TENBookingSystem.DTO.Booking;
using TENBookingSystem.DTO.Uploads;

namespace TENBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseApiController
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("book")]
        public async Task<ActionResult<Result<BookingResponse>>> DoBooking(BookingCommand booking)
        {
            return await Mediator.Send(booking);
        }
        [HttpGet("bookList")]
        public async Task<ActionResult<Result<List<BookingListDto>>>> bookList()
        {
            BookingListCommand obj = new BookingListCommand();
            return await Mediator.Send(obj);
        }
        [HttpDelete("cancel/{bookingReferenceId}/{MemerId}")]
        public async Task<ActionResult<Result<BookingResponse>>> DoCancel(int bookingReferenceId, int MemerId)
        {
            CancelBookingCommand bookingCommand = new CancelBookingCommand()
            {
                BookingId = bookingReferenceId,
                MemeberId = MemerId
            };
            return await Mediator.Send(bookingCommand);
        }

    }
}
