using Protivity.Assignment.CustomerApi.Repository.DataModel;

namespace Protivity.Assignment.CustomerApi.Repository
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// get by guid id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Customer> GetCustomerByIdAsync(Guid id);

        /// <summary>
        /// get data by integer id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Customer>> GetCustomerByAgeAsync(int id);
        /// <summary>
        /// get all data 
        /// </summary>
        /// <returns></returns>
        Task<List<Customer>> GetAllCustomerAsync();
        /// <summary>
        /// create new data
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<Customer> CreateCustomerAsync(Customer customer);

        /// <summary>
        /// update existing data for given Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<Customer> UpdateCustomerAsync(Guid id, Customer customer);

    }
}
