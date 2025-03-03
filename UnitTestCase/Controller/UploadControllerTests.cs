using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TENBookingSystem.API.Controllers;
using TENBookingSystem.Application.Commands.Bookings;
using TENBookingSystem.Application.Commands.Uploads;
using TENBookingSystem.Application.Core;
using TENBookingSystem.DTO.Booking;
using TENBookingSystem.DTO.Uploads;

namespace UnitTestCase.Controller
{
    public class UploadControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UploadController _controller;

        public UploadControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UploadController(_mediatorMock.Object);
        }

        private IFormFile CreateMockFile(string fileName, string contentType, byte[] data)
        {
            var stream = new MemoryStream(data);
            var file = new FormFile(stream, 0, data.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
            return file;
        }

        [Fact]
        public async Task UploadMembers_ReturnsSuccessResult()
        {
            // Arrange
            var mockFile = CreateMockFile("members.csv", "text/csv", new byte[] { 1, 2, 3 });

            var expectedResponse = new Result<UplaodFileResult> { IsSuccess = true };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UploadMemberCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.UploadMembers(mockFile);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Result<UplaodFileResult>>>(result);
            var actualResult = Assert.IsType<Result<UplaodFileResult>>(actionResult.Value);
            Assert.True(actualResult.IsSuccess);
        }

        [Fact]
        public async Task UploadInventory_ReturnsSuccessResult()
        {
            // Arrange
            var mockFile = CreateMockFile("inventory.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", new byte[] { 1, 2, 3 });

            var expectedResponse = new Result<UplaodFileResult> { IsSuccess = true };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UploadInventoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.UploadInventory(mockFile);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Result<UplaodFileResult>>>(result);
            var actualResult = Assert.IsType<Result<UplaodFileResult>>(actionResult.Value);
            Assert.True(actualResult.IsSuccess);
        }

        [Fact]
        public async Task UploadMembers_ReturnsBadRequest_WhenFileIsNull()
        {
            // Act
            var result = await _controller.UploadMembers(null);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("File is empty", actionResult.Value);
        }

        [Fact]
        public async Task UploadInventory_ReturnsBadRequest_WhenFileIsEmpty()
        {
            // Arrange
            var emptyFile = CreateMockFile("empty.txt", "text/plain", new byte[0]);

            // Act
            var result = await _controller.UploadInventory(emptyFile);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("File is empty", actionResult.Value);
        }
    }
}
