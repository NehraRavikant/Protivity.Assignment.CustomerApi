using Protivity.Assignment.CustomerApi.Repository.DataModel;

namespace Protivity.Assignment.CustomerApi.Common.Helpers
{
    public interface IMapper
    {
        /// <summary>
        /// Map DataModel to Dto
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        CustomerDto MapModelToDto(Customer customer);

        /// <summary>
        /// Map Dto to dataModel
        /// </summary>
        /// <param name="customerDto"></param>
        /// <returns></returns>
        Customer MapDtoToModel(CustomerDto customerDto);
    }
}
