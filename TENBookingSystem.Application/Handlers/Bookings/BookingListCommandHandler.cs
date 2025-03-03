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
using TENBookingSystem.DTO.Booking;
using Microsoft.EntityFrameworkCore;
using static TENBookingSystem.DTO.Enum.BookingsEnum;

namespace TENBookingSystem.Application.Handlers.Bookings
{
    public class BookingListCommandHandler : IRequestHandler<BookingListCommand, Result<List<BookingListDto>>>
    {
        private readonly DataContext _dataContext;
        public BookingListCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Result<List<BookingListDto>>> Handle(BookingListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<BookingListDto> result = new List<BookingListDto>();
                //getting list of booking from current date to future date 
                var bookings = _dataContext.Bookings
                                               .Include(m=> m.Member)
                                               .Include(i=> i.Inventory)
                                               .Where(a => a.BookingDate >= DateOnly.FromDateTime(DateTime.Now)).ToList();
                if (bookings.Any())
                {
                    result = bookings.Select(x => new BookingListDto()
                    {
                        BookingId = x.Id,
                        MemberName = x.Member.Name,
                        Title = x.Inventory.Title,
                        BookingDate = x.BookingDate.ToString(),
                        BookingStatus = HelperUtility.GetEnumDescriptionByValue<BookingStatus>(x.BookingStatus)
                    }).ToList();
                    return Result<List<BookingListDto>>.Success(result);

                } else
                {  
                    return Result<List<BookingListDto>>.Success(result);
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
