using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PartsOrderAPI.Controllers;
using PartsOrderAPI.DTO;
using PartsOrderAPI.Models;
using PartsOrderAPI.Services;
using Serilog;
using Xunit;
using FluentAssertions;

namespace PartsOrderAPI.Tests
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _controller;
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<ILogger> _mockLogger;

        public OrdersControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _mockLogger = new Mock<ILogger>();
            _controller = new OrdersController(_mockOrderService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public void GetOrders_ShouldReturnListOfOrders()
        {
            // Arrange
            var mockOrders = new List<Order>
            {
                new Order
                {
                    LineItems = new List<LineItem>
                    {
                        new LineItem { PartId = 1, Quantity = 10 }
                    }
                }
            };
            _mockOrderService.Setup(service => service.GetOrders()).Returns(mockOrders);

            // Act
            var result = _controller.GetOrders();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<IEnumerable<OrderDto>>().Subject;
            returnValue.Should().HaveCount(1);
            var order = returnValue.First();
            order.LineItems.Should().HaveCount(1);
            var lineItem = order.LineItems.First();
            lineItem.PartId.Should().Be(1);
            lineItem.Quantity.Should().Be(10);
        }

        [Fact]
        public void GetOrders_Exception_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockOrderService.Setup(service => service.GetOrders()).Throws(new Exception("Test exception"));

            // Act
            var result = _controller.GetOrders();

            // Assert
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            statusCodeResult.Value.Should().Be("An error occurred while retrieving the orders.");
        }

        [Fact]
        public void CreateOrder_ShouldCreateNewOrder()
        {
            // Arrange
            var newOrderDto = new OrderDto
            {
                LineItems = new List<LineItemDto>
                {
                    new LineItemDto { PartId = 1, Quantity = 10 }
                }
            };
            var createdOrder = new Order
            {
                LineItems = new List<LineItem>
                {
                    new LineItem { PartId = 1, Quantity = 10 }
                }
            };
            _mockOrderService.Setup(service => service.CreateOrder(newOrderDto)).Returns(createdOrder);

            // Act
            var result = _controller.CreateOrder(newOrderDto);

            // Assert
            var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var returnValue = createdAtActionResult.Value.Should().BeAssignableTo<OrderDto>().Subject;
            returnValue.LineItems.Should().HaveCount(1);
            var lineItem = returnValue.LineItems.First();
            lineItem.PartId.Should().Be(1);
            lineItem.Quantity.Should().Be(10);
        }

        [Fact]
        public void CreateOrder_Exception_ShouldReturnInternalServerError()
        {
            // Arrange
            var newOrderDto = new OrderDto
            {
                LineItems = new List<LineItemDto>
                {
                    new LineItemDto { PartId = 1, Quantity = 10 }
                }
            };
            _mockOrderService.Setup(service => service.CreateOrder(newOrderDto)).Throws(new Exception("Test exception"));

            // Act
            var result = _controller.CreateOrder(newOrderDto);

            // Assert
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            statusCodeResult.Value.Should().Be("An error occurred while creating the order.");
        }

        [Fact]
        public void CreateOrder_InvalidModel_ShouldReturnBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("LineItems", "Required");

            var newOrderDto = new OrderDto
            {
                LineItems = null
            };

            // Act
            var result = _controller.CreateOrder(newOrderDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
