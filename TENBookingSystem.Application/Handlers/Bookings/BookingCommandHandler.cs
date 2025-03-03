using CsvHelper.Configuration;
using CsvHelper;
using MediatR;
using System.Globalization;
using TENBookingSystem.Application.Commands.Uploads;
using TENBookingSystem.Application.Core;
using TENBookingSystem.Entities.Members;
using TENBookingSystem.Persistence;
using TENBookingSystem.Entities.Inventorys;
using TENBookingSystem.DTO.Uploads;
using TENBookingSystem.Application.Utility;
using TENBookingSystem.Application.Commands.Bookings;
using TENBookingSystem.Entities.Bookings;
using System.Diagnostics;

namespace TENBookingSystem.Application.Handlers.Bookings
{
    public class BookingCommandHandler : IRequestHandler<BookingCommand, Result<BookingResponse>>
    {
        private readonly DataContext _dataContext;
        public BookingCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Result<BookingResponse>> Handle(BookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                BookingResponse objResponse = new BookingResponse();
                var BookingCount = _dataContext.Bookings.Where(a=> a.MemberId == request.BookingDtoRequest.MemberId && a.BookingDate >= DateOnly.FromDateTime(DateTime.Now)).Count();
                if (BookingCount < 2 )
                {
                    Booking entity = new Booking();
                    entity.MemberId = request.BookingDtoRequest.MemberId;
                    entity.InventoryId = request.BookingDtoRequest.InventoryId;
                    entity.BookingDate = DateOnly.FromDateTime(request.BookingDtoRequest.BookingDate);                    
                    // as a paramenter accepting Enum value from frond end based on payment confirmation
                    // Confirmed = 1,
                    // Processed = 2,
                    // Cancelled = 3
                    entity.BookingStatus = request.BookingDtoRequest.BookingStatus;
                    // here we can add more fields as we need like created by and all 

                    await _dataContext.Bookings.AddAsync(entity);
                    await _dataContext.SaveChangesAsync();
                    objResponse.StatusMessage = "Booking Successfuly done and details has send to your registered emaild Id";
                    return Result<BookingResponse>.Success(objResponse);

                } else
                {
                    objResponse.ErrorMessage = "Booking is not allowed because you have reached the max limit of 2";
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
