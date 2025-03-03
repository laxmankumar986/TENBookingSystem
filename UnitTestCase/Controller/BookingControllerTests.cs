using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TENBookingSystem.API.Controllers;
using TENBookingSystem.Application.Commands.Bookings;
using TENBookingSystem.Application.Core;
using TENBookingSystem.DTO.Booking;
using TENBookingSystem.DTO.Uploads;

namespace UnitTestCase.Controller
{
    public class BookingControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BookingController _controller;

        public BookingControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BookingController(_mediatorMock.Object);
        }

        [Fact]
        public async Task DoBooking_ReturnsExpectedResult()
        {
            // Arrange
            var command = new BookingCommand();
            var expectedResponse = new Result<BookingResponse> { IsSuccess = true };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.DoBooking(command);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Result<BookingResponse>>>(result);
            var actualResult = Assert.IsType<Result<BookingResponse>>(actionResult.Value);
            Assert.True(actualResult.IsSuccess);
        }

        [Fact]
        public async Task BookList_ReturnsListOfBookings()
        {
            // Arrange
            var expectedResponse = new Result<List<BookingListDto>>
            {
                IsSuccess = true,
                Value = new List<BookingListDto> { new BookingListDto() }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<BookingListCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.bookList();

            // Assert
            var actionResult = Assert.IsType<ActionResult<Result<List<BookingListDto>>>>(result);
            var actualResult = Assert.IsType<Result<List<BookingListDto>>>(actionResult.Value);
            Assert.True(actualResult.IsSuccess);
            Assert.Single(actualResult.Value);
        }

        [Fact]
        public async Task DoCancel_ReturnsExpectedResult()
        {
            // Arrange
            int bookingReferenceId = 1;
            int memberId = 100;

            var expectedResponse = new Result<BookingResponse> { IsSuccess = true };

            _mediatorMock
                .Setup(m => m.Send(It.Is<CancelBookingCommand>(
                    cmd => cmd.BookingId == bookingReferenceId && cmd.MemeberId == memberId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.DoCancel(bookingReferenceId, memberId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Result<BookingResponse>>>(result);
            var actualResult = Assert.IsType<Result<BookingResponse>>(actionResult.Value);
            Assert.True(actualResult.IsSuccess);
        }
    }
}
