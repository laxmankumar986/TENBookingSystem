using MediatR;
using Microsoft.EntityFrameworkCore;
using TENBookingSystem.Application.Commands.Bookings;
using TENBookingSystem.Application.Core;
using TENBookingSystem.DTO.Enum;
using TENBookingSystem.DTO.Uploads;
using TENBookingSystem.Persistence;

namespace TENBookingSystem.Application.Handlers.Bookings
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Result<BookingResponse>>
    {
        private readonly DataContext _dataContext;
        public CancelBookingCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Result<BookingResponse>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                BookingResponse objResponse = new BookingResponse();
                var GetBooking = _dataContext.Bookings.FirstOrDefaultAsync(a => a.MemberId == request.MemeberId && a.Id == request.BookingId).Result;
                if (GetBooking != null)
                {
                    // marking booking status as cancelled
                    GetBooking.BookingStatus = (int)BookingsEnum.BookingStatus.Cancelled;
                    // here we can put more field like action taken by user id and all
                    _dataContext.Bookings.Update(GetBooking);
                    await _dataContext.SaveChangesAsync();
                    objResponse.StatusMessage = "Booking cancelled succesfully";
                    return Result<BookingResponse>.Success(objResponse);

                }
                else
                {
                    objResponse.ErrorMessage = "No booking found for cancellation with givem memebr Id and booking id";
                    return Result<BookingResponse>.Success(objResponse);
                }

            }
            catch (Exception ex)
            {
                throw ex;
                // we can use Rest exception here, using custome middleware for custome exception handling
            }
        }
    }
}
