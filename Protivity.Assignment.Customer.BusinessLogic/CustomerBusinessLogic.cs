using Microsoft.Extensions.Logging;
using Protivity.Assignment.CustomerApi.BusinessLogic.Interface;
using Protivity.Assignment.CustomerApi.Common;
using Protivity.Assignment.CustomerApi.Common.Helpers;
using Protivity.Assignment.CustomerApi.Repository;
using System.Net;

namespace Protivity.Assignment.CustomerApi.BusinessLogic
{
    public class CustomerBusinessLogic : ICustomerBusinessLogic
    {
        private readonly IMapper mapper;
        private readonly ICustomerRepository repository;
        private readonly ILogger<CustomerBusinessLogic> logger;
        public CustomerBusinessLogic(IMapper _mapper, ICustomerRepository _repository, ILogger<CustomerBusinessLogic> _logger)
        {
            mapper = _mapper;
            repository = _repository;
            logger = _logger;
        }



        /// <summary>
        /// get all customer
        /// </summary>
        /// <returns></returns>
        public async Task<List<CustomerDto>> GetAllCustomers()
        {
            List<CustomerDto> customers = new List<CustomerDto>();
            try
            {
                logger.LogInformation("Getting all details of customer from Repository.");
                var customerData = await repository.GetAllCustomerAsync();
                if (customerData != null)
                {
                    foreach (var customer in customerData)
                    {
                        customers.Add(mapper.MapModelToDto(customer));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return customers;
        }

        /// <summary>
        /// get by id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CustomerDto> GetCustomerById(Guid customerId)
        {
            var customer = await repository.GetCustomerByIdAsync(customerId);
            if(customer == null)
            {
                return null;
            }
            return mapper.MapModelToDto(customer);
        }

        /// <summary>
        /// get by age
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        public async Task<List<CustomerDto>> GetCustomerByAge(int age)
        {
            List<CustomerDto> customers = new List<CustomerDto>();
            try
            {
                logger.LogInformation("Getting all customers details.");
                var customerData = await repository.GetCustomerByAgeAsync(age);
                if (customerData != null)
                {
                    foreach (var customer in customerData)
                    {
                        customers.Add(mapper.MapModelToDto(customer));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return customers;
        }

        /// <summary>
        /// Add new Customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<CustomerDto> AddCustomer(CustomerDto customer)
        {
            logger.LogInformation("Map Dto to DataModel");
            var customerData = mapper.MapDtoToModel(customer);
            var addedData = await repository.CreateCustomerAsync(customerData);
            
            return mapper.MapModelToDto(addedData);
        }

        /// <summary>
        /// update data for existing customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<CustomerDto> UpdateCustomer(Guid id, CustomerDto customer)
        {
            try 
            {
                var customerData = mapper.MapDtoToModel(customer);
                var entity = await repository.UpdateCustomerAsync(id, customerData);
                var updatedRecord = mapper.MapModelToDto(entity);
                return updatedRecord;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// generate profile image
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public async Task<string?> GeneratCustomerProfileImageAsync(string fullName)
        {
            var encodedName = WebUtility.UrlEncode(fullName);
            var imageApiBaseUrl = "https://ui-avatars.com/api/";
            var url = $"{imageApiBaseUrl}?name={encodedName}&format=svg";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var svgImageData = await response.Content.ReadAsStringAsync();
                    return svgImageData;
                }
                else
                {
                    logger.LogWarning(response.StatusCode.ToString());
                    return null;
                }
            }
        }
    }
}