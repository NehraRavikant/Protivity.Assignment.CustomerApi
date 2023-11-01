using Microsoft.Extensions.Logging;
using Moq;
using Protivity.Assignment.CustomerApi.Controllers;
using Protivity.Assignment.CustomerApi.BusinessLogic.Interface;
using Protivity.Assignment.CustomerApi.Common;
using Protivity.Assignment.CustomerApi.Repository.DataModel;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Protivity.Assignment.CustomerApi.Test
{
    public class CustomerControllerTests
    {
        [Fact]
        public async Task UpdateCustomer_ShouldReturnAssignable()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var patchData = new CustomerDto { FullName = "Rohan", DateOfBirth = new DateOnly(2001, 1, 1) };

            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerBusinessLogic = new Mock<ICustomerBusinessLogic>();

            var existingCustomer = new CustomerDto
            {
                Id = customerId,
                FullName = "Richard Marthin",
                DateOfBirth = new DateOnly(1990, 2, 1),
                ProfileImage = ""
            };
            var updatedCustomer = new CustomerDto
            {
                Id = customerId,
                FullName = "Rohan",
                DateOfBirth = new DateOnly(2001, 1, 1),
                ProfileImage = ""
            };

            mockCustomerBusinessLogic.Setup(b => b.GetCustomerById(customerId)).ReturnsAsync(existingCustomer);
            mockCustomerBusinessLogic.Setup(b => b.UpdateCustomer(customerId, patchData)).ReturnsAsync(updatedCustomer);
             
           

            var controller = new CustomersController(mockLogger.Object, mockCustomerBusinessLogic.Object);

            // Act
            var result = await controller.UpdateCustomer(customerId, patchData);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeAssignableTo<CustomerDto>();
            // Add more specific assertions for the updated customer if needed.
        }

        [Fact]
        public async Task UpdateCustomer_ShouldReturnNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var patchData = new CustomerDto { FullName = "Rohan", DateOfBirth = new DateOnly(2001, 1, 1) };

            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerBusinessLogic = new Mock<ICustomerBusinessLogic>();

            var existingCustomer = new CustomerDto
            {
                Id = customerId,
                FullName = "Richard Marthin",
                DateOfBirth = new DateOnly(1990, 2, 1),
                ProfileImage = ""
            };
            mockCustomerBusinessLogic.Setup(b => b.GetCustomerById(customerId)).ReturnsAsync((Guid id) => null);
            mockCustomerBusinessLogic.Setup(b => b.UpdateCustomer(customerId, patchData)).ReturnsAsync(existingCustomer) ;
            var controller = new CustomersController(mockLogger.Object, mockCustomerBusinessLogic.Object);

            // Act
            var result = await controller.UpdateCustomer(customerId, patchData);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            mockCustomerBusinessLogic.Verify(d => d.UpdateCustomer(customerId, patchData), Times.Never());
            // Add more specific assertions for the updated customer if needed.
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnCreated()
        {
            // Arrange
            var customerDto = new CustomerDto
            {
                FullName = "Himanshu Chawla",
                DateOfBirth = new DateOnly(1993, 2, 6)
            };

            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerBusinessLogic = new Mock<ICustomerBusinessLogic>();

            // Mock the external API call to generate profile image
            mockCustomerBusinessLogic.Setup(service => service.GeneratCustomerProfileImageAsync(customerDto.FullName))
                .ReturnsAsync("<svg>profile_image</svg>");

            // Mock AddCustomer method and return a valid Customer data
            mockCustomerBusinessLogic.Setup(service => service.AddCustomer(It.IsAny<CustomerDto>()))
                .ReturnsAsync((CustomerDto c) => c);

            // Pass the mock logger and service to the controller constructor
            var controller = new CustomersController(mockLogger.Object, mockCustomerBusinessLogic.Object);

            // Act
            var result = await controller.CreateCustomer(customerDto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>(); // Asserts that it returns CreatedAtAction

            // Verify the CreatedAtAction result contains correct route name and values
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.ActionName.Should().Be(nameof(CustomersController.GetCustomerById));
            createdAtActionResult.RouteValues.Should().ContainKey("id");
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnBadRequest()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerBusinessLogic = new Mock<ICustomerBusinessLogic>();

            //set customerdto as null
            CustomerDto customerDto = null;

            // Pass the mock logger and service to controller constructor
            var controller = new CustomersController(mockLogger.Object, mockCustomerBusinessLogic.Object);

            // Act
            var result = await controller.CreateCustomer(customerDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>(); // Asserts that it returns BadRequest
        }

        [Fact]
        public async Task GetCustomersByAge__WithAge_ShouldReturnOk()
        {
            // Arrange
            var age = 55; 
            var today = DateTime.Today;
            var birthYear = today.Year - age;

            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerBusinessLogic = new Mock<ICustomerBusinessLogic>();

            // Mock the customer service to return a list of customers
            var customers = new List<CustomerDto>
            {
                new CustomerDto {Id = Guid.NewGuid(), FullName = "TestName1", DateOfBirth = new DateOnly(birthYear, 1, 1), ProfileImage = "<svg>profile_image</svg>" },
                new CustomerDto {Id = Guid.NewGuid(), FullName = "TestName2", DateOfBirth = new DateOnly(birthYear, 2, 1), ProfileImage = "<svg>profile_image</svg>" },
                new CustomerDto {Id = Guid.NewGuid(), FullName = "TestName3", DateOfBirth = new DateOnly(birthYear - 3, 3, 1), ProfileImage = "<svg>profile_image</svg>"}, // Should not be included
            };

            mockCustomerBusinessLogic.Setup(service => service.GetCustomerByAge(age))
                .ReturnsAsync(customers);

            // Pass the mock logger and service to controller constructor
            var controller = new CustomersController(mockLogger.Object, mockCustomerBusinessLogic.Object);

            // Act
            var result = await controller.GetCustomersByAge(age);

            // Assert
            result.Should().BeOfType<OkObjectResult>(); // check resultType
            var okResult = result as OkObjectResult;
            var resultList = okResult.Value as List<CustomerDto>; // check return type
            resultList.Should().HaveCount(3);// match count
        }

        [Fact]
        public async Task GetCustomersByAge__WithAgeLessThanZer0_ShouldReturnOk()
        {
            // Arrange
            var age = -1;
            var today = DateTime.Today;
            var birthYear = today.Year - age;

            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerBusinessLogic = new Mock<ICustomerBusinessLogic>();

            // Mock the customer service to return a list of customers
            var customers = new List<CustomerDto>
            {
                new CustomerDto {Id = Guid.NewGuid(), FullName = "TestName1", DateOfBirth = new DateOnly(birthYear, 1, 1), ProfileImage = "<svg>profile_image</svg>" },
                new CustomerDto {Id = Guid.NewGuid(), FullName = "TestName2", DateOfBirth = new DateOnly(birthYear, 2, 1), ProfileImage = "<svg>profile_image</svg>" },
                new CustomerDto {Id = Guid.NewGuid(), FullName = "TestName3", DateOfBirth = new DateOnly(birthYear - 3, 3, 1), ProfileImage = "<svg>profile_image</svg>"}, // Should not be included
            };

            mockCustomerBusinessLogic.Setup(service => service.GetCustomerByAge(age))
                .ReturnsAsync(customers);

            // Pass the mock logger and service to controller constructor
            var controller = new CustomersController(mockLogger.Object, mockCustomerBusinessLogic.Object);

            // Act
            var result = await controller.GetCustomersByAge(age);

            // Assert
            result.Should().BeOfType<BadRequestResult>(); // should be bad request as date was less than zero
        }

        [Fact]
        public async Task GetCustomers_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerBusinessLogic = new Mock<ICustomerBusinessLogic>();

            // Mock the customer service to return a list of customers
            var customers = new List<CustomerDto>
            {
                new CustomerDto {Id = Guid.NewGuid(), FullName = "TestName1", DateOfBirth = new DateOnly(1999, 1, 1), ProfileImage = "<svg>profile_image</svg>" },
                new CustomerDto {Id = Guid.NewGuid(), FullName = "TestName2", DateOfBirth = new DateOnly(1999, 2, 1), ProfileImage = "<svg>profile_image</svg>" },
                new CustomerDto {Id = Guid.NewGuid(), FullName = "TestName3", DateOfBirth = new DateOnly(1999, 3, 1), ProfileImage = "<svg>profile_image</svg>"}, // Should not be included
            };

            mockCustomerBusinessLogic.Setup(service => service.GetAllCustomers())
                 .ReturnsAsync(customers);

            // Pass the mock logger and service to controller constructor
            var controller = new CustomersController(mockLogger.Object, mockCustomerBusinessLogic.Object);

            // Act
            var result = await controller.GetCustomers();

            // Assert
            result.Should().BeOfType<OkObjectResult>(); // Asserts that it returns Ok
            var okResult = result as OkObjectResult;
            var resultList = okResult.Value as List<CustomerDto>;

            // Verify that the correct number of customers is returned
            resultList.Should().HaveCount(3); // Assuming 3 customers were mocked
        }

        [Fact]
        public async Task GetCustomers_WithNoCustomers_ShouldReturnEmptyList()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerBusinessLogic = new Mock<ICustomerBusinessLogic>();
            // Mock the customer service to return an empty list
            var customers = new List<CustomerDto>();

            mockCustomerBusinessLogic.Setup(service => service.GetAllCustomers())
                .ReturnsAsync(customers);

            // Pass the mock logger and service to controller constructor
            var controller = new CustomersController(mockLogger.Object, mockCustomerBusinessLogic.Object);

            // Act
            var result = await controller.GetCustomers();

            // Assert
            result.Should().BeOfType<OkObjectResult>(); // Asserts that it returns Ok
            var okResult = result as OkObjectResult;
            var resultList = okResult.Value as List<CustomerDto>;

            // Verify that the result list is empty when no customers are available
            resultList.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCustomersById_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerBusinessLogic = new Mock<ICustomerBusinessLogic>();
            var guid = Guid.NewGuid();

            var customer = new CustomerDto { Id = guid, FullName = "TestName1", DateOfBirth = new DateOnly(1999, 1, 1), ProfileImage = "<svg>profile_image</svg>" };
            // Mock the customer service to return a list of customers
            var customers = new List<CustomerDto>
            {
                new CustomerDto {Id = guid, FullName = "TestName1", DateOfBirth = new DateOnly(1999, 1, 1), ProfileImage = "<svg>profile_image</svg>" },
                new CustomerDto {Id = Guid.NewGuid(), FullName = "TestName2", DateOfBirth = new DateOnly(1999, 2, 1), ProfileImage = "<svg>profile_image</svg>" },
                new CustomerDto {Id = Guid.NewGuid(), FullName = "TestName3", DateOfBirth = new DateOnly(1999, 3, 1), ProfileImage = "<svg>profile_image</svg>"}, // Should not be included
            };

            mockCustomerBusinessLogic.Setup(service => service.GetCustomerById(guid))
                 .ReturnsAsync(customer);

            // Pass the mock logger and service to controller constructor
            var controller = new CustomersController(mockLogger.Object, mockCustomerBusinessLogic.Object);

            // Act
            var result = await controller.GetCustomerById(guid);

            // Assert
            result.Should().BeOfType<OkObjectResult>(); // Asserts that it returns Ok
            var okResult = result as OkObjectResult;
            var resultData = okResult.Value as CustomerDto;
            resultData.Id.Equals(guid); //should match guid
        }

        [Fact]
        public async Task GetCustomersById_ShouldNotFound()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerBusinessLogic = new Mock<ICustomerBusinessLogic>();
            var guid = Guid.NewGuid();

            var customer = new CustomerDto { Id = Guid.NewGuid(), FullName = "TestName1", DateOfBirth = new DateOnly(1999, 1, 1), ProfileImage = "<svg>profile_image</svg>" };
            // Mock the customer service to return a list of customers
            

            mockCustomerBusinessLogic.Setup(service => service.GetCustomerById(guid))
                 .ReturnsAsync((Guid id) => null);

            // Pass the mock logger and service to controller constructor
            var controller = new CustomersController(mockLogger.Object, mockCustomerBusinessLogic.Object);

            // Act
            var result = await controller.GetCustomerById(guid);

            // Assert
            result.Should().BeOfType<NotFoundResult>(); // Assert to NotFound
        }

    }
           
}