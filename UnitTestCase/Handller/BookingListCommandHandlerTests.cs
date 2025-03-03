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
using TENBookingSystem.Entities.Members;
using TENBookingSystem.Entities.Inventorys;
using TENBookingSystem.Entities.Bookings;

namespace UnitTestCase.Handller
{
    public class BookingListCommandHandlerTests
    {
        private DataContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            return new DataContext(options);
        }

        [Fact]
        public async Task Handle_ShouldReturnBookings_WhenBookingsExist()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            // Seed test data
            var member = new Member { Id = 1, Name = "John Doe" };
            var inventory = new Inventory { Id = 1, Title = "Item A" };
            dbContext.Members.Add(member);
            dbContext.Inventorys.Add(inventory);
            dbContext.Bookings.Add(new Booking
            {
                Id = 1,
                MemberId = 1,
                InventoryId = 1,
                BookingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), // Future booking
                BookingStatus = (int)BookingStatus.Confirmed
            });
            await dbContext.SaveChangesAsync();

            var handler = new BookingListCommandHandler(dbContext);
            var request = new BookingListCommand();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
            Assert.Equal("John Doe", result.Value[0].MemberName);
            Assert.Equal("Item A", result.Value[0].Title);
            Assert.NotNull(result.Value[0].BookingDate);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoFutureBookingsExist()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            // No future bookings added
            var handler = new BookingListCommandHandler(dbContext);
            var request = new BookingListCommand();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_OnDatabaseError()
        {
            // Arrange
            var mockDbContext = new Mock<DataContext>();
            var handler = new BookingListCommandHandler(mockDbContext.Object);

            mockDbContext.Setup(x => x.Bookings).Throws(new Exception("Database error"));

            var request = new BookingListCommand();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
