using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TENBookingSystem.Application.Commands.Bookings;
using TENBookingSystem.Application.Handlers.Bookings;
using TENBookingSystem.DTO.Booking;
using TENBookingSystem.Entities.Bookings;
using TENBookingSystem.Persistence;

namespace UnitTestCase.Handller
{
    public class BookingCommandHandlerTests
    {
        private DataContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            return new DataContext(options);
        }

        [Fact]
        public async Task Handle_ShouldAllowBooking_WhenBookingCountIsLessThan2()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var handler = new BookingCommandHandler(dbContext);

            var memberId = 1;
            var inventoryId = 101;
            var request = new BookingCommand
            {
                BookingDtoRequest = new BookingDto
                {
                    MemberId = memberId,
                    InventoryId = inventoryId,
                    BookingDate = DateTime.Now.AddDays(1),
                    BookingStatus = 1 // Confirmed
                }
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Booking Successfuly done and details has send to your registered emaild Id", result.Value.StatusMessage);

            var bookingCount = dbContext.Bookings.Count();
            Assert.Equal(1, bookingCount);
        }

        [Fact]
        public async Task Handle_ShouldNotAllowBooking_WhenBookingCountIs2OrMore()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var handler = new BookingCommandHandler(dbContext);

            var memberId = 2;
            var inventoryId = 102;
            var bookingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

            // Add two existing bookings for the member
            dbContext.Bookings.Add(new Booking { MemberId = memberId, InventoryId = inventoryId, BookingDate = bookingDate, BookingStatus = 1 });
            dbContext.Bookings.Add(new Booking { MemberId = memberId, InventoryId = inventoryId, BookingDate = bookingDate, BookingStatus = 1 });
            await dbContext.SaveChangesAsync();

            var request = new BookingCommand
            {
                BookingDtoRequest = new BookingDto
                {
                    MemberId = memberId,
                    InventoryId = inventoryId,
                    BookingDate = DateTime.Now.AddDays(1),
                    BookingStatus = 1 // Confirmed
                }
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Booking is not allowed because you have reached the max limit of 2", result.Value.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenDatabaseFails()
        {
            // Arrange
            var mockDbContext = new Mock<DataContext>();
            var handler = new BookingCommandHandler(mockDbContext.Object);

            var request = new BookingCommand
            {
                BookingDtoRequest = new BookingDto
                {
                    MemberId = 3,
                    InventoryId = 103,
                    BookingDate = DateTime.Now.AddDays(1),
                    BookingStatus = 1 // Confirmed
                }
            };

            mockDbContext.Setup(x => x.Bookings).Throws(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
