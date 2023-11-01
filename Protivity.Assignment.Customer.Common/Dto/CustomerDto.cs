using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protivity.Assignment.CustomerApi.Common
{
    public class CustomerDto
    {
        /// <summary>
        /// customerId
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Full Name Customer
        /// </summary>
        [Required(ErrorMessage = "Customer Full Name is Mandatody.")]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        /// <summary>
        /// Date of birth of customer
        /// </summary>
        [Required(ErrorMessage = "DOB is Mandatory")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        /// <summary>
        /// image of customer
        /// </summary>
        public string? ProfileImage { get; set; }
    }
}
