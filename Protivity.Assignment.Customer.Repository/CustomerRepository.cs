using Microsoft.EntityFrameworkCore;
using Protivity.Assignment.CustomerApi.Repository.DataModel;

namespace Protivity.Assignment.CustomerApi.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _context;

        public CustomerRepository(CustomerContext context)
        {
                _context = context;
        }

        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }



        /// <summary>
        /// get all customer
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Customer>> GetAllCustomerAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        /// <summary>
        /// get all customer of specified age
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Customer>> GetCustomerByAgeAsync(int age)
        {
            return  _context.Customers.Where(cust => (DateTime.Now.Year - cust.DateOfBirth.Year) == age).ToList();
        }

        /// <summary>
        /// get customer of given Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(cust => cust.CustomerId == id);
        }

        /// <summary>
        /// update existing customer of specific id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Customer> UpdateCustomerAsync(Guid id, Customer customer)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);

            if (existingCustomer == null)
            {
                return null;
            }

            existingCustomer.FullName = customer.FullName;
            existingCustomer.DateOfBirth = customer.DateOfBirth;

            await _context.SaveChangesAsync();
            return existingCustomer;
        }
    }
}