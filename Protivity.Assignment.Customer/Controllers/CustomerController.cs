using Microsoft.AspNetCore.Mvc;
using Protivity.Assignment.CustomerApi.BusinessLogic.Interface;
using Protivity.Assignment.CustomerApi.Common;
using System.ComponentModel.DataAnnotations;

namespace Protivity.Assignment.CustomerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> logger;
        private readonly ICustomerBusinessLogic customerBusinessLogic;
        public CustomersController(ILogger<CustomersController> _logger, ICustomerBusinessLogic _customerBusinessLogic)
        {
            logger = _logger;
            customerBusinessLogic = _customerBusinessLogic;

        }

        /// <summary>
        /// get all customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            logger.LogInformation("Getting all customers details.");
            var customers = await customerBusinessLogic.GetAllCustomers();
            return Ok(customers);
        }

        /// <summary>
        /// get customer by guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}", Name = "GetCustomerById")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await customerBusinessLogic.GetCustomerById(id);

            if (customer == null)
            {
                logger.LogWarning("No Customer found with this id.");
                return NotFound();
            }
            logger.LogInformation("Customer data received.");
            return Ok(customer);
        }

        /// <summary>
        /// get customer by age
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        [HttpGet("{age:int}", Name = "GetCustomersByAge")]
        public async Task<IActionResult> GetCustomersByAge(int age)
        {
            // date should be greate than zero
            if (age < 0)
            {
                logger.LogError("You have entered age below zero.");
                return BadRequest();
            }
            var customers = await customerBusinessLogic.GetCustomerByAge(age);

            return Ok(customers);
        }

        /// <summary>
        /// Add new customer
        /// </summary>
        /// <param name="customerDto"></param>
        /// <returns></returns>
        /// <exception cref="InvalidModelException"></exception>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            //check if dto is null
            if (customerDto == null)
            {
                logger.LogError("No customer details provided.");
                return BadRequest("Customer data is not valid.");
            }
            // Check if the ModelState is valid
            if (!ModelState.IsValid)
            {
                logger.LogError("Model state is not valid");
                return BadRequest(ModelState);
            }
            var existingCustomer = await customerBusinessLogic.GetCustomerById(customerDto.Id);
            if (existingCustomer != null)
            {
                logger.LogWarning("Customer exist with this id.");
                return Problem("Customer Already Exists.");
            }

            // Validate the model
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(customerDto, new ValidationContext(customerDto), validationResults, true))
            {
                logger.LogError("Invalid Model. Please check model properties and try again.");
                throw new InvalidModelException(validationResults);
            }
            // call image generator Api
            var svgData = await customerBusinessLogic.GeneratCustomerProfileImageAsync(customerDto.FullName);
            //check if image generated
            if (svgData == null)
            {
                logger.LogError("Image Generator not reponding properly. Please try again later.");
                return Problem("Image generator service is down. Please contact Administrator.");
            }
            // Create a new Customer entity
            customerDto.ProfileImage = svgData;
            var createdCustomer = await customerBusinessLogic.AddCustomer(customerDto);

            return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);

        }

        /// <summary>
        /// update customer data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchData"></param>
        /// <returns></returns>
        [HttpPatch("{id:guid}", Name = "UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerDto patchData)
        {
            if (patchData == null)
            {
                logger.LogError("Patch data is null.");
                return BadRequest("Invalid patch data.");
            }
            //check if customer exists
            var existingCustomer = await customerBusinessLogic.GetCustomerById(id);
            if (existingCustomer == null)
            {
                logger.LogError("No customer exist with this Id. Please try with different Id.");
                return NotFound();
            }
            var updatedCustomer  = await customerBusinessLogic.UpdateCustomer(id, patchData);
            return Ok(updatedCustomer);
        }

    }
}