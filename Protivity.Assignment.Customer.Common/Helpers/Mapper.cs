using Protivity.Assignment.CustomerApi.Common.Helpers;
using Protivity.Assignment.CustomerApi.Repository.DataModel;

namespace Protivity.Assignment.CustomerApi.Common
{
    public class Mapper : IMapper
    {
        /// <summary>
        /// Map Dto object to DataModel Object
        /// </summary>
        /// <param name="customerDto"></param>
        /// <returns></returns>
        public Customer MapDtoToModel(CustomerDto customerDto)
        {
            return new Customer {
                CustomerId = customerDto.Id,
                FullName = customerDto.FullName,
                DateOfBirth = customerDto.DateOfBirth,
                ProfileImage = customerDto.ProfileImage
            };
        }

        /// <summary>
        /// map dataModel to Dto object 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public CustomerDto MapModelToDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.CustomerId,
                FullName = customer.FullName,
                DateOfBirth = customer.DateOfBirth,
                ProfileImage = customer.ProfileImage
            };
        }
    }
}
