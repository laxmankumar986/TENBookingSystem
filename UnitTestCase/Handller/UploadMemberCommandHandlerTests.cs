using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TENBookingSystem.Application.Commands.Uploads;
using TENBookingSystem.Application.Handlers.Uploads;
using TENBookingSystem.Entities.Members;
using TENBookingSystem.Persistence;

namespace UnitTestCase.Handller
{
    public class UploadMemberCommandHandlerTests
    {
        private DataContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            return new DataContext(options);
        }

        private IFormFile CreateMockCsvFile(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, "file", "test.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };
        }

        [Fact]
        public async Task Handle_ShouldUploadValidCsvFileSuccessfully()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var handler = new UploadMemberCommandHandler(dbContext);

            string csvContent = "Id,Name,SurnName,BookingCount,DateJoined\n" +
                                "1,John,Doe,2,2024-03-01\n" +
                                "2,Jane,Doe,1,2024-03-02";

            var mockFile = CreateMockCsvFile(csvContent);
            var request = new UploadMemberCommand(mockFile);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Member data uploaded successfully", result.Value.StatusMessage);
            Assert.Equal(2, dbContext.Members.Count());
        }

        [Fact]
        public async Task Handle_ShouldFailOnInvalidDateFormat()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var handler = new UploadMemberCommandHandler(dbContext);

            string csvContent = "Id,Name,SurnName,BookingCount,DateJoined\n" +
                                "1,John,Doe,2,03-01-2024\n" +  // Incorrect date format
                                "2,Jane,Doe,1,2024-03-02";

            var mockFile = CreateMockCsvFile(csvContent);
            var request = new UploadMemberCommand(mockFile);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Contains("Row Number 1Is having invalid date format", result.Value.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenFileIsEmpty()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var handler = new UploadMemberCommandHandler(dbContext);

            var mockFile = CreateMockCsvFile(""); // Empty file
            var request = new UploadMemberCommand(mockFile);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Contains("Is having invalid date format", result.Value.ErrorMessage); // Since it's empty, validation should fail
        }

        [Fact]
        public async Task Handle_ShouldThrowException_OnDatabaseError()
        {
            // Arrange
            var mockDbContext = new Mock<DataContext>();
            var handler = new UploadMemberCommandHandler(mockDbContext.Object);

            var mockFile = CreateMockCsvFile("Id,Name,SurnName,BookingCount,DateJoined\n1,John,Doe,2,2024-03-01");

            var request = new UploadMemberCommand(mockFile);

            mockDbContext.Setup(x => x.Members.AddRangeAsync(It.IsAny<IEnumerable<Member>>(), It.IsAny<CancellationToken>()))
                         .Throws(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
