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
using TENBookingSystem.Entities.Inventorys;
using TENBookingSystem.Persistence;

namespace UnitTestCase.Handller
{
    public class UploadInventoryCommandHandlerTests
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
            var handler = new UploadInventoryCommandHandler(dbContext);

            string csvContent = "Id,Title,Descritpion,RemainingCount,ExpireDate\n" +
                                "1,Item1,Description1,10,2025-03-01\n" +
                                "2,Item2,Description2,5,2025-04-01";

            var mockFile = CreateMockCsvFile(csvContent);
            var request = new UploadInventoryCommand(mockFile);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Inventory data uploaded successfully", result.Value.StatusMessage);
            Assert.Equal(2, dbContext.Inventorys.Count());
        }

        [Fact]
        public async Task Handle_ShouldFailOnInvalidDateFormat()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var handler = new UploadInventoryCommandHandler(dbContext);

            string csvContent = "Id,Title,Descritpion,RemainingCount,ExpireDate\n" +
                                "1,Item1,Description1,10,03-01-2025\n" +  // Incorrect date format
                                "2,Item2,Description2,5,2025-04-01";

            var mockFile = CreateMockCsvFile(csvContent);
            var request = new UploadInventoryCommand(mockFile);

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
            var handler = new UploadInventoryCommandHandler(dbContext);

            var mockFile = CreateMockCsvFile(""); // Empty file
            var request = new UploadInventoryCommand(mockFile);

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
            var handler = new UploadInventoryCommandHandler(mockDbContext.Object);

            var mockFile = CreateMockCsvFile("Id,Title,Descritpion,RemainingCount,ExpireDate\n1,Item1,Description1,10,2025-03-01");

            var request = new UploadInventoryCommand(mockFile);

            mockDbContext.Setup(x => x.Inventorys.AddRangeAsync(It.IsAny<IEnumerable<Inventory>>(), It.IsAny<CancellationToken>()))
                         .Throws(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
