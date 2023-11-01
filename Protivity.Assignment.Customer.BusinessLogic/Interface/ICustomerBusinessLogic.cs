using Protivity.Assignment.CustomerApi.Common;
using Protivity.Assignment.CustomerApi.Repository.DataModel;

namespace Protivity.Assignment.CustomerApi.BusinessLogic.Interface
{
    public interface ICustomerBusinessLogic
    {
        /// <summary>
        /// get all customer
        /// </summary>
        /// <returns></returns>
        Task<List<CustomerDto>> GetAllCustomers();

        /// <summary>
        /// get customer by specifc id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<CustomerDto> GetCustomerById(Guid customerId);

        /// <summary>
        /// get customer by specifc id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<List<CustomerDto>> GetCustomerByAge(int age);


        /// <summary>
        /// add new customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<CustomerDto> AddCustomer(CustomerDto customer);

        /// <summary>
        /// update existing customer details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<CustomerDto> UpdateCustomer(Guid id, CustomerDto customer);

        /// <summary>
        /// generate user profile image
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        Task<string?> GeneratCustomerProfileImageAsync(string fullName);
    }
}
