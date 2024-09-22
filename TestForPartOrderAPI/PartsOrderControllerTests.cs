using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PartsOrderAPI.Controllers;
using PartsOrderAPI.DTO;
using PartsOrderAPI.Models;
using PartsOrderAPI.Services;
using Serilog;
using Xunit;

namespace PartsOrderAPI.Tests
{
    public class PartsControllerTests
    {
        private readonly PartsController _controller;
        private readonly Mock<IPartService> _mockPartService;
        private readonly Mock<ILogger> _mockLogger;

        public PartsControllerTests()
        {
            _mockPartService = new Mock<IPartService>();
            _mockLogger = new Mock<ILogger>();
            _controller = new PartsController(_mockPartService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public void GetParts_ShouldReturnListOfParts()
        {
            // Arrange
            var mockParts = new List<Part>
            {
                new Part
                {
                    Description = "Part1",
                    Price = 100.0m,
                    Quantity = 10
                }
            };
            _mockPartService.Setup(service => service.GetParts()).Returns(mockParts);

            // Act
            var result = _controller.GetParts();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<IEnumerable<PartDto>>().Subject;
            returnValue.Should().HaveCount(1);
            returnValue.First().Description.Should().Be("Part1");
            returnValue.First().Price.Should().Be(100.0m);
            returnValue.First().Quantity.Should().Be(10);
        }

        [Fact]
        public void GetParts_Exception_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockPartService.Setup(service => service.GetParts()).Throws(new Exception("Test exception"));

            // Act
            var result = _controller.GetParts();

            // Assert
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            statusCodeResult.Value.Should().Be("An error occurred while retrieving the parts.");
        }

        [Fact]
        public void CreatePart_ShouldCreateNewPart()
        {
            // Arrange
            var newPartDto = new PartDto
            {
                Description = "Part1",
                Price = 100.0m,
                Quantity = 10
            };
            var newPart = new Part
            {
                Description = newPartDto.Description,
                Price = newPartDto.Price,
                Quantity = newPartDto.Quantity
            };
            _mockPartService.Setup(service => service.CreatePart(newPartDto)).Returns(newPart);

            // Act
            var result = _controller.CreatePart(newPartDto);

            // Assert
            var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var returnValue = createdAtActionResult.Value.Should().BeAssignableTo<PartDto>().Subject;
            returnValue.Description.Should().Be("Part1");
            returnValue.Price.Should().Be(100.0m);
            returnValue.Quantity.Should().Be(10);
        }

       

        [Fact]
        public void CreatePart_InvalidModel_ShouldReturnBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Description", "Required");

            var newPartDto = new PartDto
            {
                Description = "",
                Price = 100.0m,
                Quantity = 10
            };

            // Act
            var result = _controller.CreatePart(newPartDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
    

