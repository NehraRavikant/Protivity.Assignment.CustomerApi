
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protivity.Assignment.CustomerApi.Repository.DataModel
{
    public class Customer
    {
        /// <summary>
        /// unique identifier for customer id to get and set individual value
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// string value to set the name of the customer.
        /// </summary>
        public required string FullName { get; set; }
        /// <summary>
        /// DOB of customer, contains only date
        /// </summary>
        public DateOnly DateOfBirth { get; set; }
        /// <summary>
        /// svg profile image of the customer generated from external link
        /// </summary>
        public string? ProfileImage { get; set; }
    }
}
