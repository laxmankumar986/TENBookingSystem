using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TENBookingSystem.DTO.Enum.BookingsEnum;
using TENBookingSystem.Application.Commands.Bookings;
using TENBookingSystem.Application.Handlers.Bookings;
using TENBookingSystem.Persistence;
using TENBookingSystem.Entities.Bookings;

namespace UnitTestCase.Handller
{
    public class CancelBookingCommandHandlerTests
    {
        private DataContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            return new DataContext(options);
        }

        [Fact]
        public async Task Handle_ShouldCancelBooking_WhenBookingExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            // Seed test data
            var booking = new Booking
            {
                Id = 1,
                MemberId = 1,
                InventoryId = 1,
                BookingStatus = (int)BookingStatus.Confirmed
            };
            dbContext.Bookings.Add(booking);
            await dbContext.SaveChangesAsync();

            var handler = new CancelBookingCommandHandler(dbContext);
            var request = new CancelBookingCommand { MemeberId = 1, BookingId = 1 };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            var updatedBooking = await dbContext.Bookings.FindAsync(1);
            Assert.True(result.IsSuccess);
            Assert.Equal((int)BookingStatus.Cancelled, updatedBooking.BookingStatus);
            Assert.Equal("Booking cancelled succesfully", result.Value.StatusMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenBookingDoesNotExist()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var handler = new CancelBookingCommandHandler(dbContext);
            var request = new CancelBookingCommand { MemeberId = 1, BookingId = 999 }; // Non-existent ID

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("No booking found for cancellation with givem memebr Id and booking id", result.Value.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_OnDatabaseError()
        {
            // Arrange
            var mockDbContext = new Mock<DataContext>();
            var handler = new CancelBookingCommandHandler(mockDbContext.Object);

            mockDbContext.Setup(x => x.Bookings.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Booking, bool>>>(), CancellationToken.None))
                         .Throws(new Exception("Database error"));

            var request = new CancelBookingCommand { MemeberId = 1, BookingId = 1 };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
